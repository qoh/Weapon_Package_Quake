datablock AudioProfile(QuakeHealthNormalSound)
{
	fileName = "./sounds/n_health.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeHealthLargeSound)
{
	fileName = "./sounds/l_health.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeHealthBoostSound)
{
	fileName = "./sounds/s_health.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeHealthMegaSound)
{
	fileName = "./sounds/m_health.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock ItemData(QuakeHealthNormalItem)
{
	shapeFile = "./shapes/q3_healthsphere.dts";
	uiName = "Health Normal";

	emap = 1;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	doColorShift = 1;
	colorShiftColor = "1 1 1 1";
	quakeLightColor = "white";

	healthAmount = 25;
	healthRespawn = 15;
	healthSound = QuakeHealthNormalSound;
	healthBoost = 0;
};

datablock ItemData(QuakeHealthLargeItem : QuakeHealthNormalItem)
{
	uiName = "Health Large";
	colorShiftColor = "0.9 0.8 0.125 1";
	quakeLightColor = "gold";

	healthAmount = 50;
	healthSound = QuakeHealthLargeSound;
};

datablock ItemData(QuakeHealthBoostItem : QuakeHealthNormalItem)
{
	uiName = "Health Boost";
	colorShiftColor = "0.125 0.85 0.125 1";
	quakeLightColor = "green";

	healthAmount = 5;
	healthSound = QuakeHealthBoostSound;
	healthBoost = 1;
};

datablock ItemData(QuakeHealthMegaItem : QuakeHealthNormalItem)
{
	uiName = "Health Mega";
	colorShiftColor = "0.125 0.125 0.85 1";
	quakeLightColor = "blue";

	healthAmount = 100;
	healthRespawn = 60;
	healthSound = QuakeHealthMegaSound;
	healthBoost = 1;
};

function QuakeHealthNormalItem::onAdd(%this, %item)
{
	Parent::onAdd(%this, %item);
}

function QuakeHealthNormalItem::onPickup(%this, %item, %obj)
{
	if (isEventPending(%item.respawn))
	{
		return;
	}

	if (%obj.health >= (%this.healthBoost ? 200 : 100))
	{
		return;
	}

	if (isObject(%this.healthSound))
	{
		serverPlay3D(%this.healthSound, %obj.getHackPosition());
	}

	%health = %obj.health + %this.healthAmount;
	%transform = %item.getTransform();

	%item.setTransform("0 0 -1000");
	%item.respawn = %item.schedule(%this.healthRespawn * 1000, setTransform, %transform);

	%item.fadeOut();
	%item.schedule(%this.healthRespawn * 1000, fadeIn);

	if (!%this.healthBoost && %health > 100)
	{
		%health = 100;
	}

	%obj.setHealth(%health);
}

function QuakeHealthLargeItem::onPickup(%this, %item, %obj)
{
	QuakeHealthNormalItem::onPickup(%this, %item, %obj);
}

function QuakeHealthBoostItem::onPickup(%this, %item, %obj)
{
	QuakeHealthNormalItem::onPickup(%this, %item, %obj);
}

function QuakeHealthMegaItem::onPickup(%this, %item, %obj)
{
	QuakeHealthNormalItem::onPickup(%this, %item, %obj);
}