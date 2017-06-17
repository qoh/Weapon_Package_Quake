datablock AudioProfile(QuakeRLFireSound)
{
	fileName = "./sounds/rocket/rocklf1a.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile(QuakeRLExplodeSound)
{
	fileName = "./sounds/rocket/rocklx1a.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeRLLoopSound)
{
	fileName = "./sounds/rocket/rockfly.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};

datablock ParticleData(QuakeRLExplosionParticle)
{
	dragCoefficient = 3;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 700;
	lifetimeVarianceMS = 400;
	textureName = "base/data/particles/cloud";
	spinSpeed = 10.0;
	spinRandomMin = -50.0;
	spinRandomMax = 50.0;
	colors[0] = "0.9 0.9 0.6 0.9";
	colors[1] = "0.9 0.5 0.6 0.0";
	sizes[0] = 10.0;
	sizes[1] = 15.0;

	useInvAlpha = 1;
};

datablock ParticleEmitterData(QuakeRLExplosionEmitter)
{
	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 10;
	velocityVariance = 1.0;
	ejectionOffset = 3.0;
	thetaMin = 89;
	thetaMax = 90;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;

	particles = QuakeRLExplosionParticle;
	emitterNode = TenthEmitterNode;
};

datablock ParticleData(QuakeRLExplosionRingParticle)
{
	dragCoefficient = 8;
	gravityCoefficient = -0.5;
	inheritedVelFactor = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS = 40;
	lifetimeVarianceMS = 10;
	textureName = "base/data/particles/star1";
	spinSpeed = 10.0;
	spinRandomMin = -500.0;
	spinRandomMax = 500.0;
	colors[0] = "1 0.5 0.2 0.5";
	colors[1] = "0.9 0.0 0.0 0.0";
	sizes[0] = 8;
	sizes[1] = 13;

	useInvAlpha = 0;
};

datablock ParticleEmitterData(QuakeRLExplosionRingEmitter)
{
	lifeTimeMS = 50;

	ejectionPeriodMS = 1;
	periodVarianceMS = 0;
	ejectionVelocity = 5;
	velocityVariance = 0.0;
	ejectionOffset = 3.0;
	thetaMin = 0;
	thetaMax = 180;
	phiReferenceVel = 0;
	phiVariance = 360;
	overrideAdvance = 0;
	particles = QuakeRLExplosionRingParticle;
};

datablock ExplosionData(QuakeRLExplosion)
{
	explosionShape = "./shapes/explosionSphere1.dts";
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

	damageRadius = 5.6;
	radiusDamage = 100;

	impulseRadius = 5.6;
	impulseForce = 3000;
};

AddDamageType("QuakeRLDirect", '%1 blew themself up', '%1 ate %2\'s rocket', 1, 1);
AddDamageType("QuakeRLRadius", '%1 blew themself up', '%1 almost dodged %2\'s rocket', 1, 0);

datablock ProjectileData(QuakeRLProjectile)
{
	projectileShapeName = "./shapes/projectiles/q3_rocket.dts";

	explosion = QuakeRLExplosion;
	sound = QuakeRLLoopSound;

	directDamage = 100;
	directDamageType = $DamageType::QuakeRLDirect;
	radiusDamageType = $DamageType::QuakeRLRadius;

	impactImpulse = 10;
	verticalImpulse = 5;

	muzzleVelocity = 42;
	velInheritFactor = 0;

	armingDelay = 00;
	lifetime = 4000;
	fadeDelay = 3500;
	bounceElasticity = 0.5;
	bounceFriction = 0.20;
	isBallistic = 0;
	gravityMod = 0.0;

	hasLight = 1;
	lightRadius = 5.0;
	lightColor = "1 0.5 0.0";
};

//////////
// item //
//////////
datablock ItemData(QuakeRLItem)
{
	category = "Weapon"; // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./shapes/q3_rlauncher.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Rocket Launcher";
	canDrop = 1;
	isQuakeItem = 1;

	iconName = "./icons/iconw_rocket";
	image = QuakeRLImage;
};

datablock ShapeBaseImageData(QuakeRLImage)
{
	shapeFile = "./shapes/q3_rlauncher.dts";
	emap = 1;

	mountPoint = 0;
	item = QuakeRLItem;

	correctMuzzleVector = 1;
	className = "WeaponImage";

	projectile = QuakeRLProjectile;
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
	stateSound[2] = QuakeRLFireSound;
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

function QuakeRLImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, jump);
	return Parent::onFire(%this, %obj, %slot);
}
