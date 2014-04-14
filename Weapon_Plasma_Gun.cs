datablock AudioProfile(QuakePlasmaGunFireSound)
{
	fileName = "./sounds/plasma/hyprbf1a.wav";
	description = AudioClosest3d;
	preload = 1;
};

datablock AudioProfile(QuakePlasmaGunExplodeSound)
{
	fileName = "./sounds/plasma/plasmx1a.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakePlasmaGunLoopSound)
{
	fileName = "./sounds/plasma/lasfly.wav";
	description = AudioCloseLooping3d;
	preload = 1;
};

datablock staticShapeData(QuakePlasmagunFXDecal)
{
	shapeFile = "./shapes/decals/q3_plasmaboom.dts";
	doColorShift = true;
	colorShiftColor = "0.25 0.75 1 1";
};

AddDamageType("QuakePlasmaGunDirect", '%1 melted themself', '%1 was melted by %2\'s plasmagun', 1, 1);
AddDamageType("QuakePlasmaGunRadius", '%1 melted themself', '%1 was melted by %2\'s plasmagun', 1, 1);

datablock ExplosionData(QuakePlasmaGunExplosion)
{
	explosionShape = "base/data/shapes/empty.dts";
	soundProfile = QuakePlasmaGunExplodeSound;

	lifeTimeMS = 350;
	faceViewer = 1;
	explosionScale = "1 1 1";

	shakeCamera = 1;
	camShakeFreq = "5 5 5";
	camShakeAmp = "1 1 1";
	camShakeDuration = 0.5;
	camShakeRadius = 1;

	lightStartRadius = 1;
	lightEndRadius = 1;
	lightStartColor = "1 1 1 1";
	lightEndColor = "0 0 0 1";

	damageRadius = 1;
	radiusDamage = 14;

	impulseRadius = 0;
	impulseForce = 0;
};

datablock ParticleData(QuakePlasmaGunTrailParticle)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
 
	inheritedVelFactor   = 1;
	constantAcceleration = 0;
 
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 0;
 
	textureName = "./shapes/f_plasma.jpg";
	useInvAlpha = 0;
 
	spinSpeed = 10;
	spinRandomMin = -150;
	spinRandomMax = 150;
 
	colors[0] = "1 1 1 1";
	colors[1] = "1 1 1 1";
	colors[2] = "1 1 1 1";
	colors[3] = "1 1 1 1";
 
	sizes[0] = 1.35;
	sizes[1] = 1.0;
	sizes[2] = 1.35;
 	sizes[3] = 0;
 
	times[0] = 0.5;
	times[1] = 0.25;
	times[2] = 0.5;
	times[3] = 0.15;
};
 
datablock ParticleEmitterData(QuakePlasmaGunTrailEmitter)
{
   ejectionPeriodMS = 1;
   ejectionVelocity = 0;
   ejectionOffset = 0;
 
   periodVarianceMS = 0;
   velocityVariance = 0;
 
   thetaMin = 0;
   thetaMax = 90;
 
   phiReferenceVel = 0;
   phiVariance = 360;
 
   overrideAdvance = 0;
   particles = QuakePlasmaGunTrailParticle;
};

datablock ProjectileData(QuakePlasmaGunProjectile)
{
	projectileShapeName = "base/data/shapes/empty.dts";

	particleEmitter = QuakePlasmaGunTrailEmitter;
	explosion = QuakePlasmaGunExplosion;
	sound = QuakePlasmaGunLoopSound;

	directDamage = 20;
	directDamageType = $DamageType::QuakePlasmaGunDirect;
	radiusDamageType = $DamageType::QuakePlasmaGunRadius;

	impactImpulse = 0;
	verticalImpulse = 0;

	muzzleVelocity = 93;
	velInheritFactor = 0.1;

	armingDelay = 0;
	lifetime = 4000;
	fadeDelay = 3500;

	isBallistic = 0;
	gravityMod = 0;

	hasLight = 1;
	lightRadius = 2.0;
	lightColor = "0.25 0.75 1";
};

datablock ItemData(QuakePlasmaGunItem)
{
	category = "Weapon"; // Mission editor category
	className = "Weapon"; // For inventory system

	shapeFile = "./shapes/q3_plasmagun.dts";
	emap = 1;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	uiName = "Plasma Gun";
	canDrop = 1;
	isQuakeItem = 1;

	iconName = "./icons/iconw_plasma";
	image = QuakePlasmaGunImage;
};

datablock ShapeBaseImageData(QuakePlasmaGunImage)
{
	shapeFile = "./shapes/q3_plasmagun.dts";
	emap = 1;

	mountPoint = 0;
	item = QuakePlasmaGunItem;

	correctMuzzleVector = 1;
	className = "WeaponImage";

	projectile = QuakePlasmaGunProjectile;
	projectileType = Projectile;

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
	stateSound[2] = QuakePlasmaGunFireSound;
	stateScript[2] = "onFire";
	stateSequence[2] = "Fire";
	stateTimeoutValue[2] = 0.05;
	stateWaitForTimeout[2] = 1;
	stateAllowImageChange[2] = 0;
	stateTransitionOnTimeout[2] = "Wait";

	stateName[3] = "Wait";
	stateTimeoutValue[3] = 0.05;
	stateWaitForTimeout[3] = 1;
	stateAllowImageChange[3] = 0;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";
};

function QuakePlasmaGunProjectile::onCollision(%this, %obj, %col, %fade, %pos, %normal)
{
	parent::onCollision(%this, %obj, %col, %fade, %pos, %normal);
	%decal = spawnDecal(QuakePlasmagunFXDecal, vectorAdd(%pos, vectorScale(%normal, 0.02)), %normal, 1);
	%decal.setScale("5 5 1");
	if (isObject(%decal)) {
		fadeAnimationLoop(%decal, 0.5);//, 16, 0.5);
	}
}