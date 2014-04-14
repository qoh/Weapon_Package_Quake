datablock AudioProfile(QuakeMachineGunFireSound)
{
	fileName = "./sounds/machinegun/machgf1b.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile(QuakeMachineGunHit1Sound)
{
	fileName = "./sounds/machinegun/ric1.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile(QuakeMachineGunHit2Sound)
{
	fileName = "./sounds/machinegun/ric2.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile(QuakeMachineGunHit3Sound)
{
	fileName = "./sounds/machinegun/ric3.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock ItemData(QuakeMachineGunItem)
{
	category = "Weapon";
	className = "Weapon";

	shapeFile = "./shapes/q3_machinegun.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Machine Gun";
	canDrop = 0;
	isQuakeItem = 1;

	iconName = "./icons/iconw_machinegun";
	image = QuakeMachineGunImage;
};

AddDamageType("QuakeMachineGunDirect", '%1 machinegunned themself', '%1 was machinegunned by %2', 1, 1);

datablock ShapeBaseImageData(QuakeMachineGunImage)
{
	shapeFile = "./shapes/q3_machinegun.dts";
	emap = 1;

	mountPoint = 0;
	item = QuakeMachineGunItem;

	correctMuzzleVector = 1;
	className = "WeaponImage";

	armReady = 1;
	minShotTime = 90;

	stateName[0] = "Activate";
	stateSound[0] = QuakeWeaponSwitchSound;
	stateTimeoutValue[0] = 0.01;
	stateAllowImageChange[0] = 0;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateAllowImageChange[1] = 1;
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateFire[2] = 1;
	stateSound[2] = QuakeMachineGunFireSound;
	stateScript[2] = "onFire";
	stateSequence[2] = "spin";
	stateTimeoutValue[2] = 0.05;
	stateWaitForTimeout[2] = 1;
	stateAllowImageChange[2] = 0;
	stateTransitionOnTimeout[2] = "Wait";

	stateName[3] = "Wait";
	// stateSequence[3] = "spin";
	stateTimeoutValue[3] = 0.05;
	stateWaitForTimeout[3] = 1;
	stateAllowImageChange[3] = 0;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "SpinDown";

	stateName[4] = "SpinDown";
	stateSequence[4] = "stop";
	stateTimeoutValue[4] = 0.9;
	stateWaitForTimeout[4] = 0;
	stateTransitionOnTimeout[4] = "Ready";
	stateTransitionOnTriggerDown[4] = "Fire";

	raycastEnabled = 1;
	raycastRange = 100;
	raycastSpread = 1;
	raycastHitExplosion = QuakeGunProjectile;

	directDamage = 6;
	directDamageType = $DamageType::QuakeMachineGunDirect;
};

// function QuakeMachineGunImage::onFire(%this, %obj, %slot)
// {
// 	return Parent::onFire(%this, %obj, %slot);
// }

function QuakeMachinegunImage::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec)
{
	Parent::onRaycastCollision(%this, %obj, %col, %pos, %normal, %vec);
	if (!(%col.getType() & $TypeMasks::PlayerObjectType))
	{
		serverPlay3D(nameToID("QuakeMachineGunHit" @ getRandom(1, 3) @ "Sound"), %pos);
		spawnDecal(gunShotShapeData, vectorAdd(%pos, vectorScale(%normal, 0.02)), %normal, 1);
	}
}