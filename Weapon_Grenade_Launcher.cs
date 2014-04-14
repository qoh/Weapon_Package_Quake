datablock AudioProfile(QuakeGrenadeLauncherFireSound)
{
	fileName = "./sounds/grenade/grenlf1a.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeGrenadeLauncherBounce1Sound)
{
	fileName = "./sounds/grenade/hgrenb1a.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeGrenadeLauncherBounce2Sound)
{
	fileName = "./sounds/grenade/hgrenb2a.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock ExplosionData(QuakeGrenadeLauncherExplosion)
{
	soundProfile = QuakeRLExplodeSound;
	lifeTimeMS = 350;

	particleEmitter = QuakeRLExplosionEmitter;
	particleDensity = 10;
	particleRadius = 0.2;

	emitter[0] = QuakeRLExplosionRingEmitter;

	faceViewer = 1;
	explosionScale = "1 1 1";

	shakeCamera = 1;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "3.0 10.0 3.0";
	camShakeDuration = 0.5;
	camShakeRadius = 20.0;

	lightStartRadius = 10;
	lightEndRadius = 25;
	lightStartColor = "1 1 1 1";
	lightEndColor = "0 0 0 1";

	damageRadius = 7.12;
	radiusDamage = 99;

	impulseRadius = 7.12;
	impulseForce = 1280;
};

AddDamageType("QuakeGrenadeLauncher", '%1 blew himself up', '%1 was grenaded by %2', 1, 0);

datablock ProjectileData(QuakeGrenadeLauncherProjectile)
{
	projectileShapeName = "./shapes/projectiles/q3_grenade.dts";
	explosion = QuakeGrenadeLauncherExplosion;

	directDamage = 0;
	radiusDamageType = $DamageType::QuakeGrenadeLauncher;

	impactImpulse = 10;
	verticalImpulse = 5;

	muzzleVelocity = 33;
	velInheritFactor = 0.1;

	lifetime = 2500;
	armingDelay = 2500;
	fadeDelay = 2500;
	explodeOnDeath = 1;

	bounceElasticity = 0.5;
	bounceFriction = 0.20;
	isBallistic = 1;
	gravityMod = 1;

	hasLight = 1;
	lightRadius = 5.0;
	lightColor = "1 0.25 0.0";
};

//////////
// item //
//////////
datablock ItemData(QuakeGrenadeLauncherItem)
{
	category = "Weapon"; // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./shapes/q3_grenadel.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Grenade Launcher";
	canDrop = 1;
	isQuakeItem = 1;

	iconName = "./icons/iconw_grenade";
	image = QuakeGrenadeLauncherImage;
};

datablock ShapeBaseImageData(QuakeGrenadeLauncherImage)
{
	shapeFile = "./shapes/q3_grenadel.dts";
	emap = 1;

	mountPoint = 0;
	item = QuakeGrenadeLauncherItem;

	correctMuzzleVector = 1;
	className = "WeaponImage";

	projectile = QuakeGrenadeLauncherProjectile;
	projectileType = Projectile;

	armReady = 1;
	minShotTime = 750;

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
	stateSound[2] = QuakeGrenadeLauncherFireSound;
	stateScript[2] = "onFire";
	stateSequence[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateWaitForTimeout[2] = 1;
	stateAllowImageChange[2] = 0;
	stateTransitionOnTimeout[2] = "Wait";

	stateName[3] = "Wait";
	stateTimeoutValue[3] = 0.7;
	stateAllowImageChange[3] = 0;
	stateTransitionOnTimeout[3] = "Ready";
};

function QuakeGrenadeLauncherImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, jump);
	return Parent::onFire(%this, %obj, %slot);
}

function QuakeGrenadeLauncherProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	if (%col.getType() & $TypeMasks::PlayerObjectType && !%obj.bounced)
	{
		%col.damage(%obj.sourceObject, %pos, 100, $DamageType::QuakeGrenadeLauncher);
		%obj.explode();
	}
	else
	{
		%obj.bounced = 1;
		serverPlay3D(nameToID("QuakeGrenadeLauncherBounce" @ getRandom(1, 2) @ "Sound"), %pos);
	}

	Parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
}