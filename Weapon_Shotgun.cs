datablock AudioProfile(QuakeShotgunFireSound)
{
	fileName = "./sounds/shotgun/sshotf1b.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ItemData(QuakeShotgunItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/q3_shotgun.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Shotgun";
	canDrop = 1;
	isQuakeItem = 1;

	iconName = "./icons/iconw_shotgun";
	image = QuakeShotgunImage;
};

addDamageType("QuakeShotgun", '%1 gunned down themself', '%1 was gunned down by %2', 1, 1);

datablock ShapeBaseImageData(QuakeShotgunImage)
{
	shapeFile = "./shapes/q3_shotgun.dts";
	emap = 1;

	mountPoint = 0;
	item = QuakeShotgunItem;

	correctMuzzleVector = 1;
	className = "WeaponImage";

	armReady = 1;
	minShotTime = 250;

	stateName[0] = "Activate";
	stateSound[0] = QuakeWeaponSwitchSound;
	stateSequence[0] = "root";
	stateTimeoutValue[0] = 0.01;
	stateAllowImageChange[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateSequence[1] = "root";
	stateAllowImageChange[1] = 1;
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateFire[2] = 1;
	stateSound[2] = QuakeShotgunFireSound;
	stateScript[2] = "onFire";
	stateTimeoutValue[2] = 0.9;
	stateWaitForTimeout[2] = 1;
	stateAllowImageChange[2] = 0;
	stateTransitionOnTimeout[2] = "Wait";

	stateName[3] = "Wait";
	stateTimeoutValue[3] = 0.1;
	stateWaitForTimeout[3] = 1;
	stateAllowImageChange[3] = 0;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";

	raycastEnabled = 1;
	raycastRange = 100;
	raycastCount = 11;
	raycastSpread = 12;
	raycastHitExplosion = QuakeGunProjectile;

	directDamage = 10;
	directDamageType = $DamageType::QuakeShotgun;
};

function QuakeShotgunImage::onFire(%this, %obj, %slot)
{
	Parent::onFire(%this, %obj, %slot);
	%obj.playThread(0, jump);

	%obj.schedule(250, playThread, 3, shiftLeft);
	%obj.schedule(450, playThread, 3, shiftRight);
}

function QuakeShotgunImage::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec)
{
	Parent::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec);
	if (!(%col.getType() & $TypeMasks::PlayerObjectType))
	{
		serverPlay3D(nameToID("QuakeMachineGunHit" @ getRandom(1, 3) @ "Sound"), %pos);
		spawnDecal(gunShotShapeData, vectorAdd(%pos, vectorScale(%normal, 0.02)), %normal, 1);
	}
}