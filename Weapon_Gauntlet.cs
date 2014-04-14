datablock AudioProfile(QuakeGauntletFireSound)
{
	fileName = "./sounds/melee/fstrun.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};

datablock AudioProfile(QuakeGauntletLoopSound)
{
	fileName = "./sounds/melee/fsthum.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};

datablock AudioProfile(QuakeGauntletHitSound)
{
	fileName = "./sounds/melee/fstatck.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ItemData(QuakeGauntletItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/q3_gauntlet.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Gauntlet";
	canDrop = 0;
	isQuakeItem = 1;

	image = QuakeGauntletImage;
	iconName = "./icons/iconw_gauntlet";
};

addDamageType("Gauntlet", '%1 humiliated themself', '%1 was humiliated by %2', 1, 1);

datablock ShapeBaseImageData(QuakeGauntletImage)
{
	className = "WeaponImage";
	shapeFile = "./shapes/q3_gauntlet.dts";
	emap = 1;
	offset = "0 0 0.05";

	item = QuakeGauntletItem;
	armReady = 1;
	mountPoint = 0;
	minShotTime = 390;
	correctMuzzleVector = 1;

	stateName[0] = "Activate";
	stateSound[0] = QuakeWeaponSwitchSound;
	stateTimeoutValue[0] = 0.01;
	stateAllowImageChange[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateScript[1] = "onReady";
	stateAllowImageChange[1] = 1;
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateFire[2] = 1;
	stateScript[2] = "onFire";
	stateSequence[2] = "spin";
	stateTimeoutValue[2] = 0.2;
	stateWaitForTimeout[2] = 1;
	stateAllowImageChange[2] = 0;
	stateTransitionOnTimeout[2] = "Wait";

	stateName[3] = "Wait";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = 1;
	stateAllowImageChange[3] = 0;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";

	raycastEnabled = 1;
	raycastRange = 2.5;

	directDamage = 50;
	directDamageType = $DamageType::Gauntlet;
};

function QuakeGauntletImage::onMount(%this, %obj, %slot)
{
	%obj.playAudio(0, QuakeGauntletLoopSound);
}

function QuakeGauntletImage::onUnMount(%this, %obj, %slot)
{
	%obj.stopAudio(0);
	%obj.stopAudio(1);

	%obj.playingGauntletFire = 0;
}

function QuakeGauntletImage::onReady(%this, %obj, %slot)
{
	%obj.stopAudio(1);
	%obj.playingGauntletFire = 0;
}

function QuakeGauntletImage::onFire(%this, %obj, %slot)
{
	Parent::onFire(%this, %obj, %slot);

	if (!%obj.playingGauntletFire)
	{
		%obj.playAudio(1, QuakeGauntletFireSound);
		%obj.playingGauntletFire = 1;
	}
}

function QuakeGauntletImage::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec)
{
	Parent::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec);

	%obj.playThread(0, plant);
	serverPlay3D(QuakeGauntletHitSound, %pos);
}