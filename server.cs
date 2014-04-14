datablock AudioProfile(QuakeWeaponSwitchSound)
{
	fileName = "./sounds/change.wav";
	description = AudioDefault3D;
	preload = 1;
};

datablock staticShapeData(gunShotShapeData)
{
	shapeFile = "./shapes/bullethole.dts";
	doColorShift = true;
	colorShiftColor = "0.6 0.6 0.6 1";
};

datablock ParticleData(quakeGunRic1)
{
	dragCoefficient      = 0;
	gravityCoefficient   = 0;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 0;
	textureName          = "./shapes/decals/bullet1";
	spinSpeed		= 0;
	spinRandomMin		= -100.0;
	spinRandomMax		= 100.0;
	colors[0]     = "1 1 0.0 0.9";
	colors[1]     = "0.9 0.0 0.0 0.0";
	sizes[0]      = 1;
	sizes[1]      = 1;

	useInvAlpha = false;
};

datablock ParticleData(quakeGunRic2 : quakeGunRic1)
{
	textureName = "./shapes/decals/bullet2";
	lifetimeMS           = 100;
	lifetimeVarianceMS   = 0;
	gravityCoefficient   = 0;
	dragCoefficient      = 0;
	colors[0]		= "1 1 0.0 0";
	colors[1]		= "1 1 0.0 0.9";
	colors[2]		= "0.9 0.0 0.0 0.0";
	sizes[0]		= 1;
	sizes[1]		= 1;
	sizes[2]		= 1;
	times[0]		= 0.5;
	times[1]		= 1;
	spinSpeed		= 0;
	spinRandomMin		= -100.0;
	spinRandomMax		= 100.0;
};

datablock ParticleData(quakeGunRic3 : quakeGunRic1)
{
	textureName = "./shapes/decals/bullet3";
	lifetimeMS           = 150;
	lifetimeVarianceMS   = 0;
	gravityCoefficient   = 0;
	dragCoefficient      = 0;
	colors[0]		= "1 1 0.0 0";
	colors[1]		= "1 1 0.0 0.9";
	colors[2]		= "0.9 0.0 0.0 0.0";
	sizes[0]		= 1;
	sizes[1]		= 1;
	sizes[2]		= 1;
	times[0]		= 1.5;
	times[1]		= 1;
	spinSpeed		= 0;
	spinRandomMin		= -100.0;
	spinRandomMax		= 100.0;
};

datablock ParticleEmitterData(quakeGunExplosionRingEmitter : gunExplosionRingEmitter)
{
	lifeTimeMS = 9;

	ejectionPeriodMS = 3;
	periodVarianceMS = 0;
	ejectionVelocity = 0;
	velocityVariance = 0.0;
	ejectionOffset   = 0.0;
	thetaMin         = 89;
	thetaMax         = 90;
	phiReferenceVel  = 0;
	phiVariance      = 360;
	overrideAdvance = false;
	particles = "quakeGunRic1 quakeGunRic2 quakeGunRic3";
};

datablock ExplosionData(QuakeGunExplosion : GunExplosion)
{
	emitter[0] = QuakeGunExplosionRingEmitter;
	soundProfile = "";
};

datablock projectileData(QuakeGunProjectile : GunProjectile)
{
	explosion = QuakeGunExplosion;
};

exec("./Support_Raycasts.cs");
exec("./Support_LagComp.cs");
exec("./Support_Decals.cs");

exec("./Weapon_Gauntlet.cs");
exec("./Weapon_Machine_Gun.cs");
exec("./Weapon_Shotgun.cs");
exec("./Weapon_Rocket_Launcher.cs");
exec("./Weapon_Grenade_Launcher.cs");
exec("./Weapon_Railgun.cs");
exec("./Weapon_Plasma_Gun.cs");

exec("./Item_Health.cs");
exec("./Item_Armor.cs");