using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileDir {

    //ENEMIES
    public static string BasicEnemy { get { return "Prefabs/Enemies/BasicEnemy"; } }
    public static string EnemyHitParticles { get { return "Prefabs/Enemies/EnemyHitParticles"; } }
    public static string MainMenuEnemy { get { return "Prefabs/Enemies/MainMenuEnemy"; } }
    public static string RangedEnemy { get { return "Prefabs/Enemies/RangedEnemy"; } }

    //ITEMS
    public static string ArmourIcon { get { return "Sprites/armouricon"; } }
    public static string AssassinTome { get { return "Prefabs/Items/Tomes/AssassinTome"; } }
	public static string BaseArmour {get {return "Prefabs/Items/BasicArmour"; }}
	public static string BaseSkillCore  {get {return "Prefabs/Items/SkillCore"; }}
    public static string BaseTome { get { return "Prefabs/Items/Tomes/BaseTome"; } }
    public static string BasicProjectile { get { return "Prefabs/Items/Projectiles/BasicArrow"; } }
    public static string EnemyProjectile { get { return "Prefabs/Items/Projectiles/EnemyProjectile"; } }
    public static string GreaterHealingPotion { get { return "Prefabs/Items/GreaterHealingPotion"; } }
    public static string HealingItem { get { return "Prefabs/Items/HealingPotion"; } }
    public static string HealingItemIcon { get { return "Sprites/healingitem"; } }
    public static string HealingPotion { get { return "Prefabs/Items/HealingPotion"; } }
    public static string ItemPickup { get { return "Prefabs/Items/itempickup"; } }
    public static string RandomArmourPickup { get { return "Prefabs/Items/RandomArmourPickup"; } }
    public static string RandomWeaponPickup { get { return "Prefabs/Items/RandomWeaponPickup"; } }

    //MATERIALS
    public static string EnemyTransparent { get { return "Materials/EnemyTransparent"; } }

    //MISC
    public static string Blank { get { return "Prefabs/Blank"; } }
    public static string KeyPress { get { return "Prefabs/KeyPress"; } }
    public static string LevelupParticles { get { return "Prefabs/LevelupParticles"; } }
    public static string Sphere { get { return "Prefabs/Sphere"; } }
    public static string SaveDirectory { get { return "SavedGames/"; } }
    public static string SaveFile { get { return SaveDirectory + SettingsManager.SaveName; } }
    public static string ChestDir { get { return SaveFile + "/Chests"; } }

    //REQUIRED
    public static string REQUIREDINSCENES { get { return "Prefabs/UI/REQUIRED/REQUIREDINSCENES"; } }

    //SKILLS
    public static string ChainLightning { get { return "Prefabs/Skills/Lightning"; } }
    public static string FireBall { get { return "Prefabs/Items/Projectiles/Fireball"; } }

    //STATUSICONS
    public static string GetHypedStatusIcon { get { return "Prefabs/Statuses/GetHypedStatusIcon"; } }
    public static string PoisonStatusIcon { get { return "Prefabs/Statuses/PoisonStatusIcon"; } }
    public static string RageStatusIcon { get { return "Prefabs/Statuses/RageStatusIcon"; } }
    public static string SlowStatusIcon { get { return "Prefabs/Statuses/SlowStatusIcon"; } }

    //UI
    public static string BlackBG { get { return "Prefabs/UI/blackbg"; } }
    public static string BlankIcon { get { return "Sprites/nothing"; } }
	public static string BlueBG {get {return "Prefabs/UI/bluebg"; }}
    public static string ChestButton { get { return "Prefabs/Interactables/Chest/Button"; } }
    public static string ChestRow { get { return "Prefabs/Interactables/Chest/Row"; } }
    public static string EnemyDamageNum { get { return "Prefabs/Enemies/DamageNumbers"; } }
    public static string EnemyDamageNumberMask { get { return "Prefabs/Enemies/DamageNumberMask"; } }
    public static string EnemyHealthBar { get { return "Prefabs/Enemies/enemyhealthbar"; } }
    public static string EnemyIcon { get { return "Prefabs/UI/Minimap/enemyminimapicon"; } }
    public static string FadingText { get { return "Prefabs/UI/FadingText"; } }
    public static string FadingTMPro { get { return "Prefabs/UI/Fading TextmeshPro"; } }
    public static string FadingTextMid { get { return "Prefabs/UI/FadingTextMid"; } }
    public static string FadingTMProMid { get { return "Prefabs/UI/Fading TextmeshPro Mid"; } }
    public static string GroundOutline { get { return "Prefabs/UI/Minimap/GroundOutline"; } }
    public static string HighGroundOutline { get { return "Prefabs/UI/Minimap/HighGroundOutline"; } }
    public static string HoverText { get { return "Prefabs/UI/HoverText"; } }
    public static string LowGroundOutline { get { return "Prefabs/UI/Minimap/LowGroundOutline"; } }
    public static string InteractablePopup { get { return "Prefabs/Interactables/InteractablePopup"; } }
    public static string ItemIcon { get { return "Sprites/item"; } }
    public static string ItemMinimapIcon { get { return "Prefabs/UI/Minimap/itemminimapicon"; } }
    public static string MainMenuSaveButton { get { return "Prefabs/UI/SaveFiles/Save#Button"; } }
    public static string MerchantButton { get { return "Prefabs/Interactables/Merchant/MerchantButton"; } }
    public static string MerchantIcon { get { return "Prefabs/UI/Minimap/MerchantIcon"; } }
    public static string OverwriteSaveButton { get { return "Prefabs/UI/SaveFiles/OverwriteSave"; } }
    public static string PickupPopup { get { return "Prefabs/UI/PickupPopup"; } }
    public static string PlayerMinimapIcon { get { return "Prefabs/UI/Minimap/playerminimapicon"; } }
    public static string ProjectileIcon { get { return "Sprites/projectileicon"; } }
    public static string SkillButton { get { return "Prefabs/Skills/SkillButton"; } }
    public static string SkillCoreIcon { get { return "Sprites/skillcore"; } }
    public static string SkillColumn { get { return "Prefabs/Skills/SkillColumn"; } }
    public static string SkillRow { get { return "Prefabs/Skills/SkillRow"; } }
    public static string StatusDetailsText { get { return "Prefabs/UI/Inventory/StatusDetails"; } }
    public static string Text { get { return "Prefabs/text"; } }
    public static string TextMeshPro { get { return "Prefabs/UI/TMPro"; } }
    public static string TomeIcon { get { return "Sprites/tome"; } }

    //BOSSES
    public static string BossHealthBar { get { return "Prefabs/Enemies/BossHealthBar"; } }    

    //LEVEL1BOSS
    public static string L1BossColdProjectile { get { return "Prefabs/Enemies/Bosses/Level1Boss/ColdProjectile"; } }
    public static string L1BossLaserProjectile { get { return "Prefabs/Enemies/Bosses/Level1Boss/LaserProjectile"; } }

    //LEVEL 2 BOSS
    public static string Bookbait { get { return "Prefabs/Enemies/Bosses/Level2Boss/BookbaitObject"; } }
}
