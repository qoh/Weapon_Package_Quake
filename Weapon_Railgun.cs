datablock StaticShapeData(QuakeRailgunTracer)
{
	shapeFile = "./shapes/q3_railtracer.dts";

	tracerTime = 250;
	tracerSize = 2;
	tracerColor = "1 0 1 1";
	tracerThread = "root";
};

datablock staticShapeData(QuakeRailgunFXDecal)
{
	shapeFile = "./shapes/decals/q3_raildisc.dts";
	doColorShift = true;
	colorShiftColor = "0 0 0 1";
};

datablock AudioProfile(QuakeRailgunFireSound)
{
	fileName = "./sounds/railgun/railgf1a.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile(QuakeRailgunLoopSound)
{
	fileName = "./sounds/railgun/rg_hum.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};

datablock ItemData(QuakeRailgunItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/q3_railgun.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Railgun";
	canDrop = 1;
	isQuakeItem = 1;

	iconName = "./icons/iconw_railgun";
	image = QuakeRailgunImage;
};

AddDamageType("QuakeRailgun", '%1 railed themself', '%2 railed %1', 1, 1);

datablock ShapeBaseImageData(QuakeRailgunImage)
{
	className = "WeaponImage";
	shapeFile = "./shapes/q3_railgun.dts";
	emap = 1;

	mountPoint = 0;
	item = QuakeRailgunItem;
	armReady = 1;
	minShotTime = 1400;

	stateName[0] = "Activate";
	stateSound[0] = QuakeWeaponSwitchSound;
	stateSequence[1] = "effect";
	stateTimeoutValue[0] = 0.01;
	stateAllowImageChange[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateSequence[1] = "effect";
	stateAllowImageChange[1] = 1;
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateFire[2] = 1;
	stateSound[2] = QuakeRailgunFireSound;
	stateScript[2] = "onFire";
	stateSequence[2] = "fire";
	stateTimeoutValue[2] = 0.25;
	stateWaitForTimeout[2] = 1;
	stateAllowImageChange[2] = 0;
	stateTransitionOnTimeout[2] = "Wait";

	stateName[3] = "Wait";
	stateSequence[3] = "cooldown";
	stateTimeoutValue[3] = 1.25;
	stateAllowImageChange[3] = 0;
	stateTransitionOnTimeout[3] = "Ready";

	raycastEnabled = 1;
	raycastRange = 400;
	raycastTracer = QuakeRailgunTracer;

	directDamage = 100;
	directDamageType = $DamageType::QuakeRailgun;
};

function QuakeRailgunImage::onMount(%this, %obj, %slot)
{
	%obj.hitLastRail = 0;
	%obj.playAudio(%slot, QuakeRailgunLoopSound);
}

function QuakeRailgunImage::onUnMount(%this, %obj, %slot)
{
	%obj.stopAudio(%slot);
}

function QuakeRailgunImage::onFire(%this, %obj, %slot)
{
	Parent::onFire(%this, %obj, %slot);
	%obj.playThread(0, jump);
}

function QuakeRailgunImage::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec)
{
	Parent::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec);
	if (%col.getType() & $TypeMasks::PlayerObjectType)
	{
		if (%obj.hitLastRail && isObject(%obj.client))
		{
			%obj.client.assignMedal("Impressive");
		}

		%obj.hitLastRail = 1;
	}
	else
	{
		%decal = spawnDecal(QuakeRailgunFXDecal, vectorAdd(%pos, vectorScale(%normal, 0.02)), %normal, 1);
		%decal.setScale("2 2 1");
		if (isObject(%decal)) {
			fadeAnimationLoop(%decal, 0.01);
			schedule(1000, 0, fadeAnimationLoop, %decal, 0.05);
		}
		%obj.hitLastRail = 0;
	}
}

function QuakeRailgunImage::onRaycastMissed(%this, %obj, %pos, %vector)
{
	Parent::onRaycastMissed(%this, %obj, %pos, %vector);
	%obj.hitLastRail = 0;
}