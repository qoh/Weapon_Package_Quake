datablock AudioProfile(QuakeArmorShardSound)
{
	fileName = "./sounds/ar1_pkup.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeArmorLightSound)
{
	fileName = "./sounds/ar2_pkup.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock AudioProfile(QuakeArmorHeavySound)
{
	fileName = "./sounds/ar3_pkup.wav";
	description = AudioDefault3d;
	preload = 1;
};

datablock ItemData(QuakeArmorShardItem)
{
	shapeFile = "./shapes/q3_armorshard.dts";
	uiName = "Armor Shard";
	quakeLightColor = "greenGray";

	emap = 1;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;

	doColorShift = 1;
	colorShiftColor = "0.3 0.6 0.5 1";

	armorAmount = 5;
	armorRespawn = 15;
	armorSound = QuakeArmorShardSound;
};

datablock ItemData(QuakeArmorLightItem : QuakeArmorShardItem)
{
	uiName = "Armor Light";
	shapeFile = "./shapes/q3_armorfull.dts";
	colorShiftColor = "0.8 0.8 0.2 1";
	quakeLightColor = "yellow";

	armorAmount = 50;
	armorRespawn = 60;
	armorSound = QuakeArmorLightSound;
};

datablock ItemData(QuakeArmorHeavyItem : QuakeArmorShardItem)
{
	uiName = "Armor Heavy";
	shapeFile = "./shapes/q3_armorfull.dts";
	colorShiftColor = "0.8 0.2 0.2 1";
	quakeLightColor = "red";

	armorAmount = 100;
	armorRespawn = 60;
	armorSound = QuakeArmorHeavySound;
};

function QuakeArmorShardItem::onPickup(%this, %item, %obj)
{
	if (isEventPending(%item.respawn) || %obj.armor >= 200)
	{
		return;
	}

	if (isObject(%this.armorSound))
	{
		serverPlay3D(%this.armorSound, %obj.getHackPosition());
	}

	%transform = %item.getTransform();

	%item.setTransform("0 0 -1000");
	%item.respawn = %item.schedule(%this.armorRespawn * 1000, setTransform, %transform);

	%item.fadeOut();
	%item.schedule(%this.armorRespawn * 1000, fadeIn);

	%obj.addArmor(%this.armorAmount);
}

function QuakeArmorLightItem::onPickup(%this, %item, %obj)
{
	QuakeArmorShardItem::onPickup(%this, %item, %obj);
}

function QuakeArmorHeavyItem::onPickup(%this, %item, %obj)
{
	QuakeArmorShardItem::onPickup(%this, %item, %obj);
}