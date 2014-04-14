$Pref::Server::LagCompensationFrames = 75;
$Pref::Server::LagCompensationInterval = 20;

package LagCompensationPackage
{
	function GameConnection::spawnPlayer(%this)
	{
		Parent::spawnPlayer(%this);

		if (!isObject(LagCompensationManager) || !isObject(%this.player))
		{
			return;
		}

		if (!LagCompensationManager.isMember(%this.player))
		{
			LagCompensationManager.add(%this.player);
		}
	}

	function Armor::onDisabled(%this, %obj, %disabled)
	{
		if (isObject(LagCompensationManager) && LagCompensationManager.isMember(%obj))
		{
			LagCompensationManager.remove(%obj);
		}

		Parent::onDisabled(%this, %obj, %disabled);
	}
};

activatePackage("LagCompensationPackage");

function LagCompensationManager(%name)
{
	if (%name !$= "" && isObject(%name))
	{
		return nameToID(%name);
	}

	if (!isObject(MissionCleanup))
	{
		error("ERROR: LagCompensationManager cannot be initiated before MissionCleanup.");
		return 0;
	}

	%obj = new ScriptObject()
	{
		class = LagCompensationManager;
	};

	MissionCleanup.add(%obj);
	%obj.setName(%name);

	return %obj;
}

function LagCompensationManager::onAdd(%this)
{
	%this.objects = new SimSet();
}

function LagCompensationManager::onRemove(%this)
{
	%this.objects.delete();
}

function LagCompensationManager::frame(%this)
{
	cancel(%this.frame);
	%count = %this.getCount();

	if (!%count)
	{
		return;
	}

	for (%i = 0; %i < %count; %i++)
	{
		%this.getObject(%i).createCompensationFrame();
	}

	%this.frame = %this.schedule($Pref::Server::LagCompensationInterval, frame);
}

function LagCompensationManager::compensate(%this, %seconds)
{
	if (%seconds <= 0 || %this.active)
	{
		return;
	}

	%this.active = 1;
	%count = %this.getCount();

	for (%i = 0; %i < %count; %i++)
	{
		%object = %this.getObject(%i);

		if (%object.frameActive !$= "")
		{
			continue;
		}

		%frame = %object.findCompensationFrame(%seconds);

		if (%frame !$= "")
		{
			%object.frameActive = %frame;
			%object.frameRestore = %object.getTransform();

			%object.setTransform(%object.frameData[%frame]);
		}
	}
}

function LagCompensationManager::compensateForClient(%this, %client)
{
	if (%this.active || !isObject(%client) || %client.isLocal())
	{
		return;
	}

	%this.compensate(%client.getPing() / 1000);
}

function LagCompensationManager::stopCompensating(%this)
{
	if (!%this.active)
	{
		return;
	}

	%this.active = 0;
	%count = %this.getCount();

	for (%i = 0; %i < %count; %i++)
	{
		%object = %this.getObject(%i);

		if (%object.frameActive !$= "")
		{
			%object.setTransform(%object.frameRestore);

			%object.frameActive = "";
			%object.frameRestore = "";
		}
	}
}

function LagCompensationManager::add(%this, %object)
{
	%this.objects.add(%object);

	if (!isEventPending(%this.frame))
	{
		%this.frame = %this.schedule(0, frame);
	}
}

function LagCompensationManager::remove(%this, %object)
{
	if (%object.frameRestore !$= "")
	{
		%object.setTransform(%object.frameRestore);
	}

	%object.frameActive = "";
	%object.frameRestore = "";

	%this.objects.remove(%object);

	if (!%this.getCount())
	{
		cancel(%this.frame);
	}
}

function LagCompensationManager::isMember(%this, %object)
{
	return %this.objects.isMember(%object);
}

function LagCompensationManager::getCount(%this)
{
	return %this.objects.getCount();
}

function LagCompensationManager::getObject(%this, %index)
{
	return %this.objects.getObject(%index);
}

function SimObject::createCompensationFrame(%this)
{
	if (%this.frameActive !$= "")
	{
		return;
	}

	if (%this.frameIndex $= "")
	{
		%this.frameIndex = 0;
	}
	else
	{
		%this.frameIndex += 1;
		%this.frameIndex %= $Pref::Server::LagCompensationFrames;
	}

	if (%this.frameCount < $Pref::Server::LagCompensationFrames)
	{
		%this.frameCount++;
	}
	else if (%this.frameCount > $Pref::Server::LagCompensationFrames)
	{
		%this.frameCount = $Pref::Server::LagCompensationFrames;
	}

	%this.frameTime[%this.frameIndex] = $Sim::Time;
	%this.frameData[%this.frameIndex] = %this.getTransform();

	return;

	if (%this.frameIndex % 4 == 0 && %this.frameIndex > 0)
		createTracer(
			vectorAdd(%this.frameData[%this.findCompensationFrame(1000)], "0 0 2"),
			vectorAdd(%this.frameData[%this.frameIndex], "0 0 2"),
			QuakeRailgunTracer
		);

	return;

	if (!isObject(%this.ghostPlayer))
	{
		%this.ghostPlayer = new AIPlayer()
		{
			datablock = %this.getDataBlock();
		};

		MissionCleanup.add(%this.ghostPlayer);

		%this.client.player = %this.ghostPlayer;
		%this.client.applyBodyParts();
		%this.client.applyBodyColors();
		%this.client.player = %this;

		%this.ghostPlayer.playThread(1, armReadyRight);
	}

	%this.ghostPlayer.setTransform(%this.frameData[%this.findCompensationFrame(100)]);
	%this.ghostPlayer.mountImage(%this.getMountedImage(0), 0);
	%this.ghostPlayer.health = 999999;
	%this.ghostPlayer.setImageTrigger(0, %this.getImageTrigger(0));
}

function SimObject::findCompensationFrame(%this, %seconds)
{
	for (%i = 0; %i < %this.frameCount; %i++)
	{
		%frame = %this.frameIndex - %i;

		if (%frame < 0)
		{
			%frame = $Pref::Server::LagCompensationFrames + %frame;
		}

		%delta = mAbs($Sim::Time - %seconds - %this.frameTime[%frame]);

		if (%prevDelta !$= "" && %delta > %prevDelta)
		{
			break;
		}

		if (%bestDelta $= "" || %delta < %bestDelta)
		{
			%bestDelta = %delta;
			%bestFrame = %frame;
		}
	}

	return %bestFrame;
}

if (!isObject(LagCompensationManager))
{
	schedule(0, 0, LagCompensationManager, LagCompensationManager);
}