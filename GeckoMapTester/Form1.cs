using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Threading;

namespace GeckoMapTester
{
    public partial class Form1 : Form
    {
        bool stopBtnClk = false;
        bool startBtnClk = false;
        private bool GeckoConnected = false;
        private bool hasExtendedHandlerInstalled = false;
        private string reply = "false";
        private bool quitting = false;
        private uint ppadd = 588;
        private uint ppbase = 275662568;
        private uint pp2add = 6480;
        private uint pp2base = 275664296;
        private uint val1 = 16;
        private uint val2 = 32;
        private uint val3 = 8;
        private uint val4 = 40;
        private uint val5 = 4;
        private uint val6 = 4;
        private uint[] val1states = new uint[3];
        private uint[] val2states = new uint[3];
        private uint[] val3states = new uint[3];
        private uint[] val4states = new uint[3];
        private uint[] val5states = new uint[3];
        private uint[] val6states = new uint[3];
        private uint[] val7states = new uint[3];
        public uint ZAddress;
        public uint P2ZAddress;
        private uint timerAddress;
        private readonly uint basePointer = 0x106E5814;
        private readonly uint offsetOne = 0x2A4;
        private readonly uint offsetTwo = 0x280;
        private readonly uint offsetTwoAmiibo = 0x2B4;

        public TCPGecko Gecko;

        float coloraR, coloraG, coloraB, coloraA;
        float colorbR, colorbG, colorbB, colorbA;
        float colornR, colornG, colornB, colornA;

        public static uint float2Hex(float fNum) //made by lean because he's cool
        {
            byte[] buffer = BitConverter.GetBytes(fNum);
            uint t1 = (uint)buffer[3];
            t1 <<= 8;
            t1 += buffer[2];
            t1 <<= 8;
            t1 += buffer[1];
            t1 <<= 8;
            t1 += buffer[0];
            return t1;
        }

        public static float hexToFloat(uint val)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(val), 0);
        }

        public string ByteArrayToString(byte[] input)
        {
            return new UTF8Encoding().GetString(input);
        }

        uint diff = 0;

        uint point = 0;

        public Form1()
        {
            InitializeComponent();

            NameWrapper[] maps = {
                new NameWrapper("<no change>", "<no change>"),
                new NameWrapper("Urchin Underpass","Fld_Crank00_Vss"),
                new NameWrapper("Walleye Warehouse","Fld_Warehouse00_Vss"),
                new NameWrapper("Saltspray Rig","Fld_SeaPlant00_Vss"),
                new NameWrapper("Arowana Mall","Fld_UpDown00_Vss"),
                new NameWrapper("Blackbelly Skatepark","Fld_SkatePark00_Vss"),
                new NameWrapper("Camp Triggerfish","Fld_Athletic00_Vss"),
                new NameWrapper("Port Mackerel","Fld_Amida00_Vss"),
                new NameWrapper("Kelp Dome","Fld_Maze00_Vss"),
                new NameWrapper("Moray Towers","Fld_Tuzura00_Vss"),
                new NameWrapper("Bluefin Depot","Fld_Ruins00_Vss"),
                new NameWrapper("Shooting Range","Fld_ShootingRange_Shr"),
                new NameWrapper("Ancho-V Games","Fld_Office00_Vss"),
                new NameWrapper("Piranha Pit","Fld_Quarry00_Vss"),
                new NameWrapper("Flounder Heights","Fld_Jyoheki00_Vss"),
                new NameWrapper("Museum d'Alfonsino","Fld_Pivot00_Vss"),
                new NameWrapper("Mahi-Mahi Resort","Fld_Hiagari00_Vss"),
                new NameWrapper("Hammerhead Bridge","Fld_Kaisou00_Vss"),
                new NameWrapper("Urchin Underpass (Dojo)","Fld_Crank00_Dul"),
                new NameWrapper("Walleye Warehouse (Dojo)","Fld_Warehouse00_Dul"),
                new NameWrapper("Saltspray Rig (Dojo)","Fld_SeaPlant00_Dul"),
                new NameWrapper("Arowana Mall (Dojo)","Fld_UpDown00_Dul"),
                new NameWrapper("Blackbelly Skatepark (Dojo)","Fld_SkatePark00_Dul"),
                new NameWrapper("Tutorial 1","Fld_Tutorial00_Ttr"),
                new NameWrapper("Tutorial 2","Fld_TutorialShow00_Ttr"),
                new NameWrapper("Match Room","Fld_MatchRoom_Mch"),
                new NameWrapper("Octotrooper Hideout","Fld_EasyHide00_Msn"),
                new NameWrapper("Lair of the Octoballs","Fld_EasyClimb00_Msn"),
                new NameWrapper("Rise of the Octocopters","Fld_EasyJump00_Msn"),
                new NameWrapper("Gusher Gauntlet","Fld_Geyser00_Msn"),
                new NameWrapper("Floating Sponge Garden","Fld_Sponge00_Msn"),
                new NameWrapper("Propeller Lift Fortress","Fld_Propeller00_Msn"),
                new NameWrapper("Spreader Splatfest","Fld_PaintingLift00_Msn"),
                new NameWrapper("Octoling Invasion","Fld_RvlMaze00_Msn"),
                new NameWrapper("Unidentified Flying Object","Fld_OctZero00_Msn"),
                new NameWrapper("Inkrail Skyscape","Fld_InkRail00_Msn"),
                new NameWrapper("Inkvisible Avenues","Fld_Invisible00_Msn"),
                new NameWrapper("Flooder Junkyard","Fld_Dozer00_Msn"),
                new NameWrapper("Shifting Splatforms","Fld_SlideLift00_Msn"),
                new NameWrapper("Octoling Assault","Fld_RvlSkatePark00_Msn"),
                new NameWrapper("Undeniable Flying Object","Fld_OctRuins00_Msn"),
                new NameWrapper("Propeller Lift Playground","Fld_Propeller01_Msn"),
                new NameWrapper("Octosniper Ramparts","Fld_Charge00_Msn"),
                new NameWrapper("Spinning Spreaders","Fld_PaintingLift01_Msn"),
                new NameWrapper("Tumbling Splatforms","Fld_TurnLift00_Msn"),
                new NameWrapper("Octoling Uprising","Fld_RvlRuins00_Msn"),
                new NameWrapper("Unwelcome Flying Object","Fld_OctCrank00_Msn"),
                new NameWrapper("Switch Box Shake-Up","Fld_Trance00_Msn"),
                new NameWrapper("Spongy Observatory","Fld_Sponge01_Msn"),
                new NameWrapper("Pinwheel Power Plant","Fld_Fusya00_Msn"),
                new NameWrapper("Far-Flung Flooders","Fld_Dozer01_Msn"),
                new NameWrapper("Octoling Onslaught","Fld_RvlSeaPlant00_Msn"),
                new NameWrapper("Unavoidable Flying Object","Fld_OctSkatePark00_Msn"),
                new NameWrapper("Staff Roll","Fld_StaffRoll00_Stf"),
                new NameWrapper("Boss 1","Fld_BossStampKing_Bos_Msn"),
                new NameWrapper("Boss 2","Fld_BossCylinderKing_Bos_Msn"),
                new NameWrapper("Boss 3","Fld_BossBallKing_Bos_Msn"),
                new NameWrapper("Boss 4","Fld_BossMouthKing_Bos_Msn"),
                new NameWrapper("Boss 5","Fld_BossRailKing_Bos_Msn"),
                new NameWrapper("Matchroom","Fld_MatchRoom_Mch"),
            };
            NameCBox.DataSource = maps;
            NameCBox.SelectedIndex = 0;

            NameWrapper[] sceneenvsets = {
                new NameWrapper("<no change>", "<no change>"),
                new NameWrapper("Day 1","MisSkyDay01,Common"),
                new NameWrapper("Twilight 1","MisSkyTwilight,Common"),
                new NameWrapper("Day 2","MisSkyDay,Common"),
                new NameWrapper("Green","MisSkyGreen,Common"),
                new NameWrapper("Sunset","Ruins,MisTwilight"),
                new NameWrapper("Night","MisSkyNight,Common"),
                new NameWrapper("Galaxy Monitors","MisSkyGalaxy,Common"),
                new NameWrapper("Gray","MisSkyGray,Common"),
                new NameWrapper("Twilight 2","MisTwilight,Common"),
                new NameWrapper("Dozer","MisDozer,Common"),
                new NameWrapper("Battle","MisBattle"),
                new NameWrapper("Broken Monitors","MisMonitorBroken,Common"),
                new NameWrapper("Boss 1","Stampking,Common"),
                new NameWrapper("Boss 2","CylinderKing,Common"),
                new NameWrapper("Boss 3","BallKing,Common"),
                new NameWrapper("Boss 4","Mouthking,Common"),
                new NameWrapper("Boss 5","RailKing,Common"),
            };
            seCBox.DataSource = sceneenvsets;
            seCBox.SelectedIndex = 0;

            UintWrapper[] modset = {
                new UintWrapper("<no change>", 0x69696969),
                new UintWrapper("cNone", 0xFFFFFFFF),
                new UintWrapper("Turf War", 0x00000000),
                new UintWrapper("Rainmaker",0x00000001),
                new UintWrapper("Tower Control",0x00000003),
                new UintWrapper("Splatzone",0x00000002),
            };
            ModeCBox.DataSource = modset;
            ModeCBox.SelectedIndex = 0;

            NameWrapper[] juddset = {
                new NameWrapper("<no change>", "<no change>"),
                new NameWrapper("Fest", "Fest"),
                new NameWrapper("Normal","Normal"),
                new NameWrapper("Private","Private"),
                new NameWrapper("Rank","Rank"),
                new NameWrapper("Team","Team"),
            };
            JuddCBox.DataSource = juddset;
            JuddCBox.SelectedIndex = 0;

            UintWrapper[] amiiboset = {
                new UintWrapper("<no change>", 0x69696969),
                new UintWrapper("Girl", 0x00000000),
                new UintWrapper("Boy",0x00000001),
                new UintWrapper("Squid",0x00000002),
                new UintWrapper("Callie",0x00000003),
                new UintWrapper("Marie", 0x00000004),
            };
            AmiiboCBox.DataSource = amiiboset;
            AmiiboCBox.SelectedIndex = 0;

            NameWrapper[] hedset = {
                new NameWrapper("<no change>", "<no change>"),
                new NameWrapper("White Headband","First"),
                new NameWrapper("SRL Glasses","SUP001"),
                new NameWrapper("Testfire Headphone","SUP000"),
                new NameWrapper("NoHead","NoHed"),
                new NameWrapper("Octoling Scope","RVL000"),
                new NameWrapper("Seeweed Octo Scope","RVL001"),
                new NameWrapper("Squid Hairclip", "AMB000"),
                new NameWrapper("Samurai Helmet","AMB001"),
                new NameWrapper("Power Mask","AMB002"),
                new NameWrapper("Urchins Cap","CAP000"),
                new NameWrapper("Testfire Shirt","CAP001"),
                new NameWrapper("Takoroka Mesh","CAP002"),
                new NameWrapper("Fashion Cap","CAP003"),
                new NameWrapper("Squid-Stitch Cap","CAP004"),
                new NameWrapper("Squidvader Cap","CAP005"),
                new NameWrapper("Camo Mesh Cap","CAP006"),
                new NameWrapper("5-Panel Cap","CAP007"),
                new NameWrapper("Zekko Mesh","CAP008"),
                new NameWrapper("Backwards Cap","CAP009"),
                new NameWrapper("2-Stripe Mesh Cap","CAP010"),
                new NameWrapper("Jet Cap","CAP011"),
                new NameWrapper("Cycling Cap","CAP012"),
                new NameWrapper("SQUID GIRL Hat","CAP013"),
                new NameWrapper("Cycle King Cap","CAP014"),
                new NameWrapper("Legendary Cap","CAP015"),
                new NameWrapper("CoroCoro Cap","CAP016"),
                new NameWrapper("Retro Specs","EYE000"),
                new NameWrapper("Splash Goggles","EYE001"),
                new NameWrapper("Pilot Goggles","EYE002"),
                new NameWrapper("Coloured Shades","EYE003"),
                new NameWrapper("Black Arrowbands","EYE004"),
                new NameWrapper("Snorkel","EYE005"),
                new NameWrapper("White Arrowbands","EYE006"),
                new NameWrapper("Fake Contacts","EYE007"),
                new NameWrapper("18K Aviators","EYE008"),
                new NameWrapper("Full Moon Glasses","EYE009"),
                new NameWrapper("Octoglasses","EYE010"),
                new NameWrapper("Jungle Hat","HAT000"),
                new NameWrapper("Safari Hat","HAT001"),
                new NameWrapper("Camping Hat","HAT002"),
                new NameWrapper("Fugu Bell Hat","HAT003"),
                new NameWrapper("Bamboo Hat","HAT004"),
                new NameWrapper("Straw Boater","HAT005"),
                new NameWrapper("Classic Straw Boater","HAT006"),
                new NameWrapper("Treasure Hunter","HAT007"),
                new NameWrapper("B-Ball Headband","HBD001"),
                new NameWrapper("Squash Headband","HBD002"),
                new NameWrapper("Tennis Headband","HBD003"),
                new NameWrapper("Jogging Headband","HBD004"),
                new NameWrapper("Football Headband","HBD005"),
                new NameWrapper("Traditional Headband","HBD006"),
                new NameWrapper("Studio Headphones","HDP000"),
                new NameWrapper("Colourful Headphones","HDP001"),
                new NameWrapper("Noise Cancellers","HDP002"),
                new NameWrapper("Cycle Helmet","MET000"),
                new NameWrapper("Stealth Goggles","MET002"),
                new NameWrapper("Tentacles Helmet","MET003"),
                new NameWrapper("Skate Helmet","MET004"),
                new NameWrapper("Visor Skate Helmet","MET005"),
                new NameWrapper("Gas Mask","MSK000"),
                new NameWrapper("Paintball Mask","MSK001"),
                new NameWrapper("Paisley Bandana","MSK002"),
                new NameWrapper("Skull Bandana","MSK003"),
                new NameWrapper("Hero Headset Replica","MSN000"),
                new NameWrapper("Hero Helmet 1","MSN001"),
                new NameWrapper("Hero Helmet 2","MSN002"),
                new NameWrapper("Hero Helmet 3","MSN003"),
                new NameWrapper("Armour Helmet Replica","MSN004"),
                new NameWrapper("Bobble Hat","NCP000"),
                new NameWrapper("Short Beanie","NCP001"),
                new NameWrapper("Striped Beanie","NCP002"),
                new NameWrapper("Sporty Bobble Hat","NCP003"),
                new NameWrapper("Special Forces Beret","NCP004"),
                new NameWrapper("Squid Nordic","NCP005"),
                new NameWrapper("Golf Visor","VIS000"),
                new NameWrapper("FishFry Visor","VIS001"),
                new NameWrapper("Sun Visor","VIS002"),
            };
            HeadCBox.DataSource = hedset;
            HeadCBox.SelectedIndex = 0;

            NameWrapper[] cltset = {
                new NameWrapper("<no change>", "<no change>"),
                new NameWrapper("Baisc Tee","First"),
                new NameWrapper("SRL Coat","SUP001"),
                new NameWrapper("NoClothes","NoClothes"),
                new NameWrapper("Testfire Shirt","SUP000"),
                new NameWrapper("School Uniform","AMB000"),
                new NameWrapper("Samurai Jacket","AMB001"),
                new NameWrapper("Power Armour","AMB002"),
                new NameWrapper("Splatfest Tee","HAP000"),
                new NameWrapper("Black Squideye","TES001"),
                new NameWrapper("SquidForce White","TES002"),
                new NameWrapper("Sky Blue Squideye","TES003"),
                new NameWrapper("Rockenberg White","TES004"),
                new NameWrapper("Rockenberg Black","TES005"),
                new NameWrapper("Black Tee","TES006"),
                new NameWrapper("Sunny Day Tee","TES007"),
                new NameWrapper("Rainy Day Tee","TES008"),
                new NameWrapper("Reggae Tee","TES009"),
                new NameWrapper("Fugu Tee","TES010"),
                new NameWrapper("Mint Tee","TES011"),
                new NameWrapper("Grape Tee","TES012"),
                new NameWrapper("Red Vector Tee","TES013"),
                new NameWrapper("Grey Vector Tee","TES014"),
                new NameWrapper("Blue Peaks Tee","TES015"),
                new NameWrapper("Ivory Peaks Tee","TES016"),
                new NameWrapper("Squid-Stitch Tee","TES017"),
                new NameWrapper("Pirate Stripes Tee","TES018"),
                new NameWrapper("Sailor Stripes Tee","TES019"),
                new NameWrapper("White 8-Bit FishFry","TES020"),
                new NameWrapper("Black 8-Bit FishFry","TES021"),
                new NameWrapper("White Anchor Tee","TES022"),
                new NameWrapper("Black Anchor Tee","TES023"),
                new NameWrapper("White Line Tee","TES024"),
                new NameWrapper("Black Pipe Tee","TES025"),
                new NameWrapper("Carnivore Tee","TES026"),
                new NameWrapper("Pearl Tee","TES027"),
                new NameWrapper("Octo Tee","TES028"),
                new NameWrapper("Herbivore Tee","TES029"),
                new NameWrapper("White Striped LS","TEL000"),
                new NameWrapper("Black LS","TEL001"),
                new NameWrapper("Purple Camo LS","TEL002"),
                new NameWrapper("Navy Striped LS","TEL003"),
                new NameWrapper("Zekko Baseball LS","TEL004"),
                new NameWrapper("Varsity Baseball LS","TEL005"),
                new NameWrapper("Black Baseball LS","TEL006"),
                new NameWrapper("White Baseball LS","TEL007"),
                new NameWrapper("White LS","TEL008"),
                new NameWrapper("Green Striped LS","TEL009"),
                new NameWrapper("Squidmark LS","TEL010"),
                new NameWrapper("Zink LS","TEL011"),
                new NameWrapper("Striped Peaks LS","TEL012"),
                new NameWrapper("White Layered LS","TLY000"),
                new NameWrapper("Yellow Layered LS","TLY001"),
                new NameWrapper("Layered Camo LS","TLY002"),
                new NameWrapper("Black Layered LS","TLY003"),
                new NameWrapper("Zink Layered LS","TLY004"),
                new NameWrapper("Layered Anchor LS","TLY005"),
                new NameWrapper("Choco Layered LS","TLY006"),
                new NameWrapper("Layered Vector LS","TLY007"),
                new NameWrapper("Green Tee","TLY008"),
                new NameWrapper("Pink Shrimp Polo","TLY011"),
                new NameWrapper("Striped Rugby","PLO000"),
                new NameWrapper("Tricolour Rugby","PLO001"),
                new NameWrapper("Sage Green Polo","PLO002"),
                new NameWrapper("Black Polo","PLO003"),
                new NameWrapper("Cycling Shirt","PLO004"),
                new NameWrapper("Cycle King Jersey","PLO005"),
                new NameWrapper("Slipstream United","PLO006"),
                new NameWrapper("FC Albacore","PLO007"),
                new NameWrapper("Olive Ski Jacket","PLO008"),
                new NameWrapper("Varsity Hoodie","JTK000"),
                new NameWrapper("Berry Ski Jacket","JTK001"),
                new NameWrapper("Varsity Jacket","JTK002"),
                new NameWrapper("School Jersey","JTK003"),
                new NameWrapper("Green Cardigan","JTK004"),
                new NameWrapper("Black Inky Rider ","JTK005"),
                new NameWrapper("White Inky Rider","JTK006"),
                new NameWrapper("Retro Gamer Jersey","JTK007"),
                new NameWrapper("Orange Cardigan","JTK008"),
                new NameWrapper("Forge Inkling Parka","JTK009"),
                new NameWrapper("Forge Octarian Jacket","JTK010"),
                new NameWrapper("Blue Sailor Suit","JTK011"),
                new NameWrapper("White Sailor Suit","JTK012"),
                new NameWrapper("Squid Satin Jacket","JTK013"),
                new NameWrapper("Zapfish Satin Jacket","JTK014"),
                new NameWrapper("Krak-On 528","JTK015"),
                new NameWrapper("B-Ball Vest (Home)","JTK016"),
                new NameWrapper("SQUID GIRL Tunic","TNK000"),
                new NameWrapper("Grey College Sweat","TNK001"),
                new NameWrapper("Squidmark Sweat","TNK002"),
                new NameWrapper("Retro Sweat","SWT000"),
                new NameWrapper("Firefin Sweat Navy","SWT001"),
                new NameWrapper("Navy College Sweat","SWT002"),
                new NameWrapper("Reel Sweat","SWT003"),
                new NameWrapper("Anchor Sweat","SWT004"),
                new NameWrapper("Lumberjack Shirt","SWT006"),
                new NameWrapper("Rodeo Shirt","SWT007"),
                new NameWrapper("White Shirt","SHT001"),
                new NameWrapper("Urchins Jersey","SHT002"),
                new NameWrapper("Aloha Shirt","SHT003"),
                new NameWrapper("Red Check Shirt","SHT004"),
                new NameWrapper("Baby Jelly Shirt","SHT005"),
                new NameWrapper("Baseball Jersey","SHT006"),
                new NameWrapper("Grey Mixed Shirt","SHT007"),
                new NameWrapper("Vintage Check","SHT008"),
                new NameWrapper("Round Collar Shirt","SHT009"),
                new NameWrapper("Logo Aloha Shirt","SHT010"),
                new NameWrapper("Striped Shirt","SHT011"),
                new NameWrapper("Linen Shirt","SHT012"),
                new NameWrapper("Shirt and Tie","SHT013"),
                new NameWrapper("Traditional Apron","SHT014"),
                new NameWrapper("Mountain Gilet","SHT015"),
                new NameWrapper("Forest Gilet","SHT016"),
                new NameWrapper("Dark Urban Gilet","VST000"),
                new NameWrapper("Yellow Urban Gilet","VST001"),
                new NameWrapper("Squid Pattern Waistcoat","VST002"),
                new NameWrapper("Yellow Urban Gilet","VST003"),
                new NameWrapper("Squid Pattern Waistcoat","VST004"),
                new NameWrapper("Squidstar Waistcoat","VST005"),
                new NameWrapper("Camo Zip Hoodie","PRK000"),
                new NameWrapper("Green Zip Hoodie","PRK001"),
                new NameWrapper("Zekko Hoodie","PRK002"),
                new NameWrapper("CoroCoro Hoodie","PRK003"),
                new NameWrapper("Hero Jacket Replica","MSN000"),
                new NameWrapper("Armour Jacket 1","MSN001"),
                new NameWrapper("Armour Jacket 2","MSN002"),
                new NameWrapper("Armour Jacket 3","MSN003"),
                new NameWrapper("Armour Jacket Replica","MSN004"),
                new NameWrapper("Octoling Armour","RVL000"),
             };
            BodyCBox.DataSource = cltset;
            BodyCBox.SelectedIndex = 0;

            NameWrapper[] shsset = {
                new NameWrapper("<no change>", "<no change>"),
                new NameWrapper("Cream Tennis","First"),
                new NameWrapper("SRL Shoes","SUP001"),
                new NameWrapper("NoShoes","NoShoes"),
                new NameWrapper("Testfire Hi-Horses","SUP000"),
                new NameWrapper("Story Mode Shoes","MSN000"),
                new NameWrapper("Armour Boot 1","MSN001"),
                new NameWrapper("Armour Boot 2","MSN002"),
                new NameWrapper("Armour Boot 3","MSN003"),
                new NameWrapper("Armour Boot Replicas","MSN004"),
                new NameWrapper("School Shoes","AMB000"),
                new NameWrapper("Samurai Shoes","AMB001"),
                new NameWrapper("Power Boots","AMB002"),
                new NameWrapper("Biker Boots","BOT000"),
                new NameWrapper("Tan Work Boots","BOT001"),
                new NameWrapper("Red Work Boots","BOT002"),
                new NameWrapper("Blue Biker Boots","BOT003"),
                new NameWrapper("Moss-Green Wellies","BOT004"),
                new NameWrapper("Acerola Wellies","BOT005"),
                new NameWrapper("Punk Whites","BOT006"),
                new NameWrapper("Punk Cherries","BOT007"),
                new NameWrapper("Punk Yellows","BOT008"),
                new NameWrapper("Bubble Rain Boots","BOT009"),
                new NameWrapper("Snowy Down Boots","BOT010"),
                new NameWrapper("Icy Down Boots","BOT011"),
                new NameWrapper("Blueberry Casuals","CFS000"),
                new NameWrapper("Plum Casuals","CFS001"),
                new NameWrapper("White Kicks","LTS000"),
                new NameWrapper("Cherry Kicks","LTS001"),
                new NameWrapper("Turquoise Kicks","LTS002"),
                new NameWrapper("Squid Ink Brogues","LTS003"),
                new NameWrapper("Roasted Brogues","LTS004"),
                new NameWrapper("Octoling Boots","RVL000"),
                new NameWrapper("Oyster Clogs","SDL000"),
                new NameWrapper("Choco Clogs","SDL001"),
                new NameWrapper("Traditional Sandals","SDL002"),
                new NameWrapper("Red Hi-Horses","SHI000"),
                new NameWrapper("Zombie Hi-Horses","SHI001"),
                new NameWrapper("Cream Hi-Tops","SHI002"),
                new NameWrapper("Purple Hi-Horses ","SHI003"),
                new NameWrapper("Dark Green Hi-Tops","SHI004"),
                new NameWrapper("Red Hi-Tops","SHI005"),
                new NameWrapper("Gold Hi-Horses","SHI006"),
                new NameWrapper("SQUID GIRL Shoes","SHI007"),
                new NameWrapper("Shark Moccasins","SHI008"),
                new NameWrapper("Mawcasins","SHI009"),
                new NameWrapper("Pink Trainers","SHT000"),
                new NameWrapper("Orange Arrows","SHT001"),
                new NameWrapper("Neon Green Sea Slugs","SHT002"),
                new NameWrapper("White Arrows","SHT003"),
                new NameWrapper("Cyan Trainers","SHT004"),
                new NameWrapper("Blue Sea Slugs","SHT005"),
                new NameWrapper("Red Sea Slugs","SHT006"),
                new NameWrapper("Purple Sea Slugs","SHT007"),
                new NameWrapper("Crazy Arrows","SHT008"),
                new NameWrapper("Black Trainers","SHT009"),
                new NameWrapper("Blue Lo-Tops","SLO000"),
                new NameWrapper("Banana Basics","SLO001"),
                new NameWrapper("Ltd Edition Lo-Tops","SLO002"),
                new NameWrapper("White Seahorses","SLO003"),
                new NameWrapper("Orange Lo-Tops","SLO004"),
                new NameWrapper("Black Seahorses","SLO005"),
                new NameWrapper("Clownfish Basics","SLO006"),
                new NameWrapper("Yellow Seahorses","SLO007"),
                new NameWrapper("Strapping Whites","SLO008"),
                new NameWrapper("Strapping Reds","SLO009"),
                new NameWrapper("Football Studs","SLO010"),
                new NameWrapper("LE Football Studs","SLO011"),
                new NameWrapper("Blue Plimsolls","SLP000"),
                new NameWrapper("Red Plimsolls","SLP001"),
                new NameWrapper("Squid-Stitch Plimsolls","SLP002"),
                new NameWrapper("Trail Boots","TRS000"),
                new NameWrapper("Custom Trail Boots","TRS001"),
                new NameWrapper("Pro Trail Boots","TRS002"),
            };
            ShoesCBox.DataSource = shsset;
            ShoesCBox.SelectedIndex = 0;

            UintWrapper[] MainWep = {
                new UintWrapper("<no change>", 0x69696969),
                new UintWrapper("No Weapon", 0xFFFFFFFF),
                new UintWrapper("Sploosh - O - Matic", 0x00000000),
                new UintWrapper("Neo Sploosh - O - Matic",0x00000001),
                new UintWrapper("Sploosh - O - Matic 7",0x00000002),
                new UintWrapper("Splattershot Jr.",0x00000003),
                new UintWrapper("Custom Splattershot Jr.", 0x00000004),
                new UintWrapper("Splash - O - Matic",0x00000005),
                new UintWrapper("Neo Splash - O - Matic",0x00000006),
                new UintWrapper("Areospray MG", 0x00000007),
                new UintWrapper("Areospray RG", 0x00000008),
                new UintWrapper("Areospray PG",0x00000009),
                new UintWrapper("Splattershot",0x0000000A),
                new UintWrapper("Tentatek Splattershot",0x0000000B),
                new UintWrapper("Wasabi Splattershot", 0x0000000C),
                new UintWrapper("Hero Shot Replica",0x0000000D),
                new UintWrapper("Octoshot Replica",0x0000000E),
                new UintWrapper(".52 Gal", 0x0000000F),
                new UintWrapper(".52 Gal Deco", 0x00000010),
                new UintWrapper("N Zap 85",0x00000011),
                new UintWrapper("N Zap 89",0x00000012),
                new UintWrapper("N Zap 83",0x00000013),
                new UintWrapper("Splattershot Pro", 0x00000014),
                new UintWrapper("Forge Splattershot Pro",0x00000015),
                new UintWrapper("Berry Splattershot Pro",0x00000016),
                new UintWrapper(".96 Gal", 0x00000017),
                new UintWrapper(".96 Gal Deco", 0x00000018),
                new UintWrapper("Dual Squelcher",0x00000019),
                new UintWrapper("Custom Blaster",0x00000020),
                new UintWrapper("Range Blaster",0x00000021),
                new UintWrapper("Custom Range Blaster", 0x00000022),
                new UintWrapper("Grim Range Blaster",0x00000023),
                new UintWrapper("Rapid Blaster",0x00000024),
                new UintWrapper("Rapid Blaster Deco", 0x00000025),
                new UintWrapper("Rapid Blaster Pro",0x00000026),
                new UintWrapper("Rapid Blaster Pro Deco",0x00000027),
                new UintWrapper("L-3 Nozzlenose", 0x00000028),
                new UintWrapper("L-3 Nozzlenose D",0x00000029),
                new UintWrapper("H-3 Nozzlenose",0x0000002A),
                new UintWrapper("H-3 Nozzlenose D", 0x0000002B),
                new UintWrapper("Cherry H-3 Nozzlenose", 0x0000002C),
                new UintWrapper("Carbon Roller",0x0000002D),
                new UintWrapper("Carbon Roller Deco",0x0000002E),
                new UintWrapper("Splat Roller", 0x0000002F),
                new UintWrapper("Krak-On Splat Roller",0x00000030),
                new UintWrapper("CoroCoro Splat Roller",0x00000031),
                new UintWrapper("Hero Roller Replica", 0x00000032),
                new UintWrapper("Dynamo Roller",0x00000033),
                new UintWrapper("Gold Dynamo Roller",0x00000034),
                new UintWrapper("Tempered Dynamo Roller", 0x00000035),
                new UintWrapper("Inkbrush",0x00000036),
                new UintWrapper("Inkbrush Nouveau",0x00000037),
                new UintWrapper("Permanent Inkbrush", 0x00000038),
                new UintWrapper("Octobrush",0x00000039),
                new UintWrapper("Octobrush Nouveau",0x0000003A),
                new UintWrapper("Classic Squiffer", 0x0000003B),
                new UintWrapper("New Squiffer", 0x0000003C),
                new UintWrapper("Fresh Squiffer",0x0000003D),
                new UintWrapper("Splat Charger",0x0000003E),
                new UintWrapper("Kelp Splat Charger", 0x0000003F),
                new UintWrapper("Bento Splat Charger",0x00000040),
                new UintWrapper("Hero Charger Replica",0x00000041),
                new UintWrapper("Splatterscope", 0x00000042),
                new UintWrapper("Kelp Splatterscope",0x00000043),
                new UintWrapper("Bento Splatterscope",0x00000044),
                new UintWrapper("E-Liter 3K", 0x00000045),
                new UintWrapper("Custom E-Liter 3K",0x00000046),
                new UintWrapper("E-Liter 3K Scope",0x00000047),
                new UintWrapper("Custom E-Liter 3K Scope", 0x00000048),
                new UintWrapper("Bamboozler MK I",0x00000049),
                new UintWrapper("Bamboozer MK II",0x0000004A),
                new UintWrapper("Bamboozler MK III", 0x0000004B),
                new UintWrapper("Slosher", 0x0000004C),
                new UintWrapper("Slosher Deco",0x0000004D),
                new UintWrapper("Soda Slosher",0x0000004E),
                new UintWrapper("Tri-Slosher", 0x0000004F),
                new UintWrapper("Tri Slosher Nouveau",0x00000050),
                new UintWrapper("Sloshing Machine",0x00000051),
                new UintWrapper("Sloshing Machine Neo", 0x00000052),
                new UintWrapper("Mini Splatling",0x00000053),
                new UintWrapper("Zink Mini Splatling",0x00000054),
                new UintWrapper("Refurbrished Mini Splatling", 0x00000055),
                new UintWrapper("Heavy Splatling",0x00000056),
                new UintWrapper("Heavy Splatling Deco",0x00000057),
                new UintWrapper("Heavy Splatling Remix", 0x00000058),
                new UintWrapper("Hydra Splatling",0x00000059),
                new UintWrapper("Custom Hydra Splatling",0x0000005A),
                new UintWrapper("Hero Shot Lv0", 0x0000005B),
                new UintWrapper("Hero Shot Lv1", 0x0000005C),
                new UintWrapper("Hero Shot Lv2",0x0000005D),
                new UintWrapper("Hero Shot Lv3",0x0000005E),
                new UintWrapper("Hero Roller", 0x0000005F),
                new UintWrapper("Hero Charger",0x00000060),
                new UintWrapper("Octoshot Lv0",0x00000063),
                new UintWrapper("Octoshot Lv1",0x00000064),
                new UintWrapper("Octoshot Lv2", 0x00000065),
                new UintWrapper("Octoshot Lv3",0x00000066),
            };
            MainWepCBox.DataSource = MainWep;
            MainWepCBox.SelectedIndex = 0;

            UintWrapper[] SubWep = {
                new UintWrapper("<no change>", 0x69696969),
                new UintWrapper("No Sub Weapon", 0xFFFFFFFF),
                new UintWrapper("Splat Bomb", 0x00000000),
                new UintWrapper("Sucction Bomb",0x00000001),
                new UintWrapper("Burst Bomb",0x00000002),
                new UintWrapper("Seeker",0x00000003),
                new UintWrapper("Point Sensor", 0x00000004),
                new UintWrapper("Ink Mine",0x00000005),
                new UintWrapper("Sprinkler",0x00000006),
                new UintWrapper("Squid Beakon", 0x00000007),
                new UintWrapper("Splash Wall",0x00000008),
                new UintWrapper("Disruptor", 0x00000009),
            };
            SubWepCBox.DataSource = SubWep;
            SubWepCBox.SelectedIndex = 0;

            UintWrapper[] SpecialWep = {
                new UintWrapper("<no change>", 0x69696969),
                new UintWrapper("Inkzooka", 0x00000000),
                new UintWrapper("Killer Wail",0x00000001),
                new UintWrapper("Inkstike",0x00000002),
                new UintWrapper("Bubbler",0x00000003),
                new UintWrapper("Bomb Rush", 0x00000004),
                new UintWrapper("Kraken",0x00000005),
                new UintWrapper("Echo",0x00000006),
                new UintWrapper("Shachihoko", 0x00000007),
            };
            SpecialWepCBox.DataSource = SpecialWep;
            SpecialWepCBox.SelectedIndex = 0;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Configuration.Load();
            IPBox.Text = Configuration.currentConfig.lastIp;
        }

        private void IPBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Return")
                ConnectButton_Click(sender, e);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Gecko = new TCPGecko(IPBox.Text, 7331);
            try
            {
                Gecko.Connect();
            }
            catch (ETCPGeckoException exc)
            {
                MessageBox.Show("Connection to the TCPGecko failed: \n\n" + exc.Message + "\n\nCheck your network connection/firewall.", "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            Configuration.currentConfig.lastIp = IPBox.Text;
            Configuration.Save();

            uint JRAddr = Gecko.peek(0x106E975C) + 0x92D8;
            if (Gecko.peek(JRAddr) == 0x000003F2)
            {
                diff = JRAddr - 0x12CDADA0;
            }
            else
            {
                DisconnButton_Click(sender, e);
                MessageBox.Show(GeckoModLoader.Properties.Resources.FIND_DIFF_FAILED_TEXT, "Connection Aborted", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            groupBox5.Enabled = true;
            groupBox6.Enabled = true;
            ManControlBox.Enabled = true;
            CodeGroupBox.Enabled = true;
            CodeGroupBox.Enabled = true;
            AmiiboClothGroupBox.Enabled = true;
            ToolTab.Enabled = true;
            JGeckoUTab.Enabled = true;
            DisconnButton.Enabled = true;
            ConnectButton.Enabled = false;
        }

        private void DisconnButton_Click(object sender, EventArgs e)
        {
            Gecko.Disconnect();
            groupBox2.Enabled = false;
            groupBox3.Enabled = false;
            groupBox4.Enabled = false;
            groupBox5.Enabled = false;
            groupBox6.Enabled = false;
            ManControlBox.Enabled = false;
            CodeGroupBox.Enabled = false;
            AmiiboClothGroupBox.Enabled = false;
            ToolTab.Enabled = false;
            JGeckoUTab.Enabled = false;
            DisconnButton.Enabled = false;
            ConnectButton.Enabled = true;
        }

        private void PokeAllMaps(string NewMapName)
        {
            if (VerCBox.Text == "2.7.0" || VerCBox.Text == "2.8.0")
            {
                if (OnlineCheckBox.Checked && NewMapName != "<no change>")
                {
                    // Main maps
                    pokeThem(0x12AEDE8C + diff, NewMapName, 16);

                    // other maps
                    pokeThem(0x12B4BA3C + diff, NewMapName, 12);

                    // redundant maps
                    pokeThem(0x12B4D89C + diff, NewMapName, 9);

                }
                if (DojoCheckBox.Checked && NewMapName != "<no change>")
                {
                    // Dojo
                    pokeThem(0x105FB958, NewMapName, 1);
                }
            }
          
            if (VerCBox.Text == "2.12.0")
            {
                if (OnlineCheckBox.Checked && NewMapName != "<no change>")
                {
                    // Main maps
                    pokeThem(0x12B4D6E4 + diff, NewMapName, 16);
                }
                if (DojoCheckBox.Checked && NewMapName != "<no change>")
                {
                    // Dojo
                    pokeThem(0x12B55A84 + diff, NewMapName, 5);
                }
                if (ShootingRangeCheckBox.Checked && NewMapName != "<no change>")
                {
                    // Shooting Range
                    pokeThem(0x105FB958, NewMapName, 1);
                    pokeThem(0x12D1EEC8, NewMapName, 1);
                }
            }
        }

        public void PokeAllSceneEnvSetNames(string SetName)
        {
            if (SetName != "<no change>")
            {
                uint baseAddress = VerCBox.Text == "2.12.0" ? (uint)0x12B4D7C8 : 0x12B4BB20;

                uint[] offsets = { baseAddress, 0x12B4DA50, 0x12B4DCD8, 0x12B4DF60, 0x12B4E1E8, 0x12B4E470, 0x12B4E6F8, 0x12B4E980, 0x12B4EC08, 0x12B4EE90, 0x12B4F118, 0x12B4F3A0, 0x12B4F628, 0x12B4F8B0, 0x12B4FB38, 0x12B4FDC0, 0x12B4D2B8, 0x12B233F0 };

                for (uint i = 0; i < 16; i++)
                {
                    for (int num = 0; num < 18; num++)
                    {
                        writeString(offsets[num] + diff + i * 0x0288, SetName, "MisMonitorBroken,Common".Length);
                    }
                }
            }
        }

        private void pokeThem(uint startOffset, string NewMapName, int num)
        {
            for (uint i = 0; i < num; i++)
                writeString(startOffset + i * 0x288, NewMapName, "Fld_BossCylinderKing_Bos_Msn".Length);
        }

        private void PokeButton_Click(object sender, EventArgs e)
        {
            //disable online
            if (DojoCheckBox.Checked)
            {
                Gecko.poke(0x10613C2C, 0x5F476573);
                Gecko.poke(0x10613C3C, 0x756C6174);
                Gecko.poke(0x10613C4C, 0x68650000);
                Gecko.poke(0x10613C88, 0x63650000);
            }

            try
            {
                PokeAllMaps(((NameWrapper)NameCBox.SelectedItem).dataName);
            }
            catch (NullReferenceException)
            {
                PokeAllMaps(NameCBox.Text);
            }

            try
            {
                PokeAllSceneEnvSetNames(((NameWrapper)seCBox.SelectedItem).dataName);
            }
            catch (NullReferenceException)
            {
                PokeAllSceneEnvSetNames(seCBox.Text);
            }

            MessageBox.Show("Success!", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void writeString(uint offset, string s)
        {
            writeString(offset, s, s.Length);
        }
        private void writeString(uint offset, string s, int length)
        {
            uint push = 0;
            int pos = 0;
            if (offset % 4 != 0)
            {
                for (int i = 0; i < offset % 4; i++)
                {
                    push = push << 8 | s[pos++];
                }
                if (offset % 4 == 1)
                {
                    push = Gecko.peek(offset - offset % 4) & 0xFF000000 | push;
                }
                if (offset % 4 == 2)
                {
                    push = Gecko.peek(offset - offset % 4) & 0xFFFF0000 | push;
                }
                if (offset % 4 == 3)
                {
                    push = Gecko.peek(offset - offset % 4) & 0xFFFFFF00 | push;
                }
                Gecko.poke(offset, push);
                offset += offset % 4;
            }
            for (; pos < s.Length; offset += 4)
            {
                push = 0;
                if (pos + 1 == s.Length)
                {
                    push = (uint)s[pos++] << 24 | Gecko.peek(offset) & 0x00FFFFFF;
                    Gecko.poke(offset, push);
                    offset += 1;
                    break;
                }
                if (pos + 2 == s.Length)
                {
                    push = s[pos++];
                    push = push << 8 | s[pos++];
                    push = push << 16 | Gecko.peek(offset) & 0x0000FFFF;
                    Gecko.poke(offset, push);
                    offset += 2;
                    break;
                }
                if (pos + 3 == s.Length)
                {
                    push = s[pos++];
                    push = push << 8 | s[pos++];
                    push = push << 8 | s[pos++];
                    push = push << 8 | Gecko.peek(offset) & 0x000000FF;
                    Gecko.poke(offset, push);
                    break;
                }
                for (int i = 0; i < 4; i++)
                {
                    push = push << 8 | s[pos++];
                }
                Gecko.poke(offset, push);
            }
            for (; pos < length; offset += 4, pos += 4)
            {
                if (pos % 4 == 1)
                {
                    Gecko.poke(offset, Gecko.peek(offset) & 0xFF000000);
                    pos--;
                    continue;
                }
                if (pos % 4 == 2)
                {
                    Gecko.poke(offset, Gecko.peek(offset) & 0xFFFF0000);
                    pos--; pos--;
                    continue;
                }
                if (pos % 4 == 3)
                {
                    Gecko.poke(offset, Gecko.peek(offset) & 0xFFFFFF00);
                    pos--; pos--; pos--;
                    continue;
                }
                if (pos + 1 == length)
                {
                    push = Gecko.peek(offset) & 0x00FFFFFF;
                    Gecko.poke(offset, push);
                    offset += 1;
                    pos++;
                    break;
                }
                if (pos + 2 == length)
                {
                    push = Gecko.peek(offset) & 0x0000FFFF;
                    Gecko.poke(offset, push);
                    offset += 2;
                    pos += 2;
                    break;
                }
                if (pos + 3 == length)
                {
                    push = Gecko.peek(offset) & 0x000000FF;
                    Gecko.poke(offset, push);
                    offset += 3;
                    pos += 3;
                    break;
                }
                Gecko.poke(offset, 0);

            }
        }

        private void PauseButton_Click(object sender, EventArgs e)
        {

            uint[] offsets1 = { 0x10513EA8, 0x10514258, 0x1052A9F8, 0x1052A95C };

            for (int num1 = 0; num1 < 4; num1++)
            {
                writeString(offsets1[num1], "VSGame", "Mission".Length);
            }
            pokeThem(0x1052A96C, "Customize", 1);
        }

        private void VssOverStrButton_Click(object sender, EventArgs e)
        {
            uint[] vssAddress = { 0x1060CA20, 0x1060CA98 };

            for (int num2 = 0; num2 < 2; num2++)
            {
                writeString(vssAddress[num2], "VSGame", "ShootingRange".Length);
            }
        }

        public void PokeAllModes(uint SetShs)
        {
            if (SetShs != 0x69696969)
            {
                Gecko.poke(0x12D1F0DC, SetShs);
            }
        }

        private void SetSendButton_Click(object sender, EventArgs e)
        {
         PokeAllModes(((UintWrapper)ModeCBox.SelectedItem).dataVal);

            if (JuddCBox.Text != "<No Change>")
            {
                                //Fest Judd
                if (JuddCBox.Text == "Fest")
                {
                    Gecko.poke(0x12D1F0D8, 0x00000002);
                    Gecko.poke(0x12D1F0E0, 0x00000005);
                }
                //Normal Judd
                else if (JuddCBox.Text == "Normal")
                {
                    Gecko.poke(0x12D1F0D8, 0x00000000);
                    Gecko.poke(0x12D1F0E0, 0x00000007);
                }
                //Private Judd
                else if (JuddCBox.Text == "Private")
                {
                    Gecko.poke(0x12D1F0D8, 0x00000003);
                    Gecko.poke(0x12D1F0E0, 0x00000003);
                }
                //Rank Judd
                else if (JuddCBox.Text == "Rank")
                {
                    Gecko.poke(0x12D1F0D8, 0x00000001);
                    Gecko.poke(0x12D1F0E0, 0x00000006);
                }
                //Team Judd
                else if (JuddCBox.Text == "Team")
                {
                    Gecko.poke(0x12D1F0D8, 0x00000004);
                    Gecko.poke(0x12D1F0E0, 0x00000008);
                }
            }

            MessageBox.Show("Success!", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void AlphaSetColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Get colors and poke.
                coloraR = colorDialog1.Color.R;
                coloraG = colorDialog1.Color.G;
                coloraB = colorDialog1.Color.B;
                coloraA = colorDialog1.Color.A;
                try
                {
                    //current colors
                    Gecko.poke(0x12D1F180 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12D1F180 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12D1F180 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Green_Default
                    Gecko.poke(0x12674F74 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12674F74 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12674F74 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Yellow
                    Gecko.poke(0x12675334 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12675334 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12675334 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Turquoise
                    Gecko.poke(0x126756F4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126756F4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126756F4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_DarkBlue
                    Gecko.poke(0x12675AB4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12675AB4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12675AB4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Marigold
                    Gecko.poke(0x12675E74 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12675E74 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12675E74 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Soda
                    Gecko.poke(0x126765F4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126765F4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126765F4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Lilac
                    Gecko.poke(0x126769B4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126769B4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126769B4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_LumiGreen
                    Gecko.poke(0x12676D74 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12676D74 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12676D74 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Orange
                    Gecko.poke(0x12677134 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12677134 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12677134 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_NightLumiGreen
                    Gecko.poke(0x126774F4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126774F4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126774F4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_NightMarigold
                    Gecko.poke(0x126778B4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126778B4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126778B4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_OrangeBlue_Default
                    Gecko.poke(0x12683F84 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12683F84 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12683F84 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_GreenPurple
                    Gecko.poke(0x12684344 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12684344 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12684344 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkBlue
                    Gecko.poke(0x12684704 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12684704 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12684704 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkOrange
                    Gecko.poke(0x12684AC4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12684AC4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12684AC4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_BlueLime
                    Gecko.poke(0x12684E84 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12684E84 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12684E84 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkGreen
                    Gecko.poke(0x12685244 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12685244 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12685244 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_TurquoiseOrange
                    Gecko.poke(0x12685604 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12685604 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12685604 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Regular_LightBlueDarkBlue
                    Gecko.poke(0x126859C4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126859C4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126859C4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_YellowLilac_Default
                    Gecko.poke(0x12692F94 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12692F94 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12692F94 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_GreenMazenta
                    Gecko.poke(0x12693354 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12693354 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12693354 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_GreenOrange
                    Gecko.poke(0x12693714 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12693714 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12693714 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_SodaPink
                    Gecko.poke(0x12693AD4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12693AD4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12693AD4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_LightgreenBlue
                    Gecko.poke(0x12693E94 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12693E94 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12693E94 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_LumigreenPurple
                    Gecko.poke(0x12694254 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12694254 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12694254 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Gachi_DarkblueYellow
                    Gecko.poke(0x12694614 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x12694614 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x12694614 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Msn_Option_Yellow
                    Gecko.poke(0x126A1FA4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126A1FA4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126A1FA4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                    //GfxSetting_Vss_Option_BlueOrange
                    Gecko.poke(0x126B0FB4 + diff, float2Hex(coloraR / 256.0F));
                    Gecko.poke(0x126B0FB4 + diff + 0x4, float2Hex(coloraG / 256.0F));
                    Gecko.poke(0x126B0FB4 + diff + 0x8, float2Hex(coloraB / 256.0F));
                }
                catch (GeckoMapTester.ETCPGeckoException exc)
                {
                    MessageBox.Show("Failed to write color data to memory.\n\n" + exc, "Operation failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                ColorALabel.Text = coloraR.ToString() + ", " + coloraG.ToString() + ", " + coloraB.ToString();
                AlphaShowBox.BackColor = colorDialog1.Color;
            }
        }

        private void BravoSetColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Get colors and poke.
                colorbR = colorDialog1.Color.R;
                colorbG = colorDialog1.Color.G;
                colorbB = colorDialog1.Color.B;
                colorbA = colorDialog1.Color.A;
                try
                {
                    //current colors
                    Gecko.poke(0x12D1F180 + diff + 0x10, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12D1F180 + diff + 0x14, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12D1F180 + diff + 0x18, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Green_Default
                    Gecko.poke(0x12674F74 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12674F74 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12674F74 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Yellow
                    Gecko.poke(0x12675334 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12675334 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12675334 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Turquoise
                    Gecko.poke(0x126756F4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126756F4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126756F4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_DarkBlue
                    Gecko.poke(0x12675AB4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12675AB4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12675AB4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Marigold
                    Gecko.poke(0x12675E74 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12675E74 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12675E74 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Soda
                    Gecko.poke(0x126765F4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126765F4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126765F4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Lilac
                    Gecko.poke(0x126769B4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126769B4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126769B4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_LumiGreen
                    Gecko.poke(0x12676D74 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12676D74 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12676D74 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Orange
                    Gecko.poke(0x12677134 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12677134 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12677134 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_NightLumiGreen
                    Gecko.poke(0x126774F4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126774F4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126774F4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_NightMarigold
                    Gecko.poke(0x126778B4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126778B4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126778B4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_OrangeBlue_Default
                    Gecko.poke(0x12683F84 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12683F84 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12683F84 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_GreenPurple
                    Gecko.poke(0x12684344 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12684344 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12684344 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkBlue
                    Gecko.poke(0x12684704 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12684704 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12684704 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkOrange
                    Gecko.poke(0x12684AC4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12684AC4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12684AC4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_BlueLime
                    Gecko.poke(0x12684E84 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12684E84 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12684E84 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkGreen
                    Gecko.poke(0x12685244 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12685244 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12685244 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_TurquoiseOrange
                    Gecko.poke(0x12685604 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12685604 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12685604 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Regular_LightBlueDarkBlue
                    Gecko.poke(0x126859C4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126859C4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126859C4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_YellowLilac_Default
                    Gecko.poke(0x12692F94 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12692F94 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12692F94 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_GreenMazenta
                    Gecko.poke(0x12693354 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12693354 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12693354 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_GreenOrange
                    Gecko.poke(0x12693714 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12693714 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12693714 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_SodaPink
                    Gecko.poke(0x12693AD4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12693AD4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12693AD4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_LightgreenBlue
                    Gecko.poke(0x12693E94 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12693E94 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12693E94 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_LumigreenPurple
                    Gecko.poke(0x12694254 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12694254 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12694254 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Gachi_DarkblueYellow
                    Gecko.poke(0x12694614 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x12694614 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x12694614 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Msn_Option_Yellow
                    Gecko.poke(0x126A1FA4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126A1FA4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126A1FA4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                    //GfxSetting_Vss_Option_BlueOrange
                    Gecko.poke(0x126B0FB4 + diff + 0x64, float2Hex(colorbR / 256.0F));
                    Gecko.poke(0x126B0FB4 + diff + 0x68, float2Hex(colorbG / 256.0F));
                    Gecko.poke(0x126B0FB4 + diff + 0x6C, float2Hex(colorbB / 256.0F));
                }
                catch (GeckoMapTester.ETCPGeckoException exc)
                {
                    MessageBox.Show("Failed to write color data to memory.\n\n" + exc, "Operation failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                ColorBLabel.Text = colorbR.ToString() + ", " + colorbG.ToString() + ", " + colorbB.ToString();
                BravoShowBox.BackColor = colorDialog1.Color;
            }
        }

        private void NeutralSetColor_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Get colors and poke.
                colornR = colorDialog1.Color.R;
                colornG = colorDialog1.Color.G;
                colornB = colorDialog1.Color.B;
                colornA = colorDialog1.Color.A;
                try
                {
                    //current colors
                    Gecko.poke(0x12D1F180 + diff + 0x20, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12D1F180 + diff + 0x24, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12D1F180 + diff + 0x28, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Green_Default
                    Gecko.poke(0x12674F74 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12674F74 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12674F74 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Yellow
                    Gecko.poke(0x12675334 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12675334 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12675334 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Turquoise
                    Gecko.poke(0x126756F4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126756F4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126756F4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_DarkBlue
                    Gecko.poke(0x12675AB4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12675AB4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12675AB4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Marigold
                    Gecko.poke(0x12675E74 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12675E74 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12675E74 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Soda
                    Gecko.poke(0x126765F4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126765F4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126765F4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Lilac
                    Gecko.poke(0x126769B4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126769B4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126769B4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_LumiGreen
                    Gecko.poke(0x12676D74 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12676D74 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12676D74 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Orange
                    Gecko.poke(0x12677134 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12677134 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12677134 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_NightLumiGreen
                    Gecko.poke(0x126774F4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126774F4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126774F4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_NightMarigold
                    Gecko.poke(0x126778B4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126778B4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126778B4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_OrangeBlue_Default
                    Gecko.poke(0x12683F84 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12683F84 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12683F84 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_GreenPurple
                    Gecko.poke(0x12684344 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12684344 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12684344 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkBlue
                    Gecko.poke(0x12684704 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12684704 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12684704 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkOrange
                    Gecko.poke(0x12684AC4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12684AC4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12684AC4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_BlueLime
                    Gecko.poke(0x12684E84 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12684E84 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12684E84 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_PinkGreen
                    Gecko.poke(0x12685244 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12685244 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12685244 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_TurquoiseOrange
                    Gecko.poke(0x12685604 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12685604 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12685604 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Regular_LightBlueDarkBlue
                    Gecko.poke(0x126859C4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126859C4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126859C4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_YellowLilac_Default
                    Gecko.poke(0x12692F94 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12692F94 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12692F94 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_GreenMazenta
                    Gecko.poke(0x12693354 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12693354 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12693354 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_GreenOrange
                    Gecko.poke(0x12693714 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12693714 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12693714 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_SodaPink
                    Gecko.poke(0x12693AD4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12693AD4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12693AD4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_LightgreenBlue
                    Gecko.poke(0x12693E94 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12693E94 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12693E94 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_LumigreenPurple
                    Gecko.poke(0x12694254 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12694254 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12694254 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Gachi_DarkblueYellow
                    Gecko.poke(0x12694614 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x12694614 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x12694614 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Msn_Option_Yellow
                    Gecko.poke(0x126A1FA4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126A1FA4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126A1FA4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                    //GfxSetting_Vss_Option_BlueOrange
                    Gecko.poke(0x126B0FB4 + diff + 0xC8, float2Hex(colornR / 256.0F));
                    Gecko.poke(0x126B0FB4 + diff + 0xCC, float2Hex(colornG / 256.0F));
                    Gecko.poke(0x126B0FB4 + diff + 0xD0, float2Hex(colornB / 256.0F));
                }
                catch (GeckoMapTester.ETCPGeckoException exc)
                {
                    MessageBox.Show("Failed to write color data to memory.\n\n" + exc, "Operation failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                ColorNLabel.Text = colornR.ToString() + ", " + colornG.ToString() + ", " + colornB.ToString();
                NeutralShowBox.BackColor = colorDialog1.Color;
            }
        }

        private void GetColor_Click(object sender, EventArgs e)
        {
            try
            {
                coloraR = hexToFloat(Gecko.peek(0x12D1F180 + diff)) * 256;
                coloraG = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x4)) * 256;
                coloraB = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x8)) * 256;

                colorbR = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x10)) * 256;
                colorbG = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x14)) * 256;
                colorbB = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x18)) * 256;

                colornR = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x20)) * 256;
                colornG = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x24)) * 256;
                colornB = hexToFloat(Gecko.peek(0x12D1F180 + diff + 0x28)) * 256;

                if (coloraR > 255) { coloraR = 255; } //Nintendo uses color values >255 sometimes, so we gotta do that
                if (coloraG > 255) { coloraG = 255; }
                if (coloraB > 255) { coloraB = 255; }

                if (colorbR > 255) { colorbR = 255; }
                if (colorbG > 255) { colorbG = 255; }
                if (colorbB > 255) { colorbB = 255; }

                if (colornR > 255) { colornR = 255; }
                if (colornG > 255) { colornG = 255; }
                if (colornB > 255) { colornB = 255; }

            }
            catch (GeckoMapTester.ETCPGeckoException exc)
            {
                MessageBox.Show("Failed to read color data from memory.\n\n" + exc, "Operation failed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }

            ColorALabel.Text = Math.Round(coloraR).ToString() + ", " + Math.Round(coloraG).ToString() + ", " + Math.Round(coloraB).ToString()/* + "\n" + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff)) + ", " + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x4)) + "\n" + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x8))*/;
            ColorBLabel.Text = Math.Round(colorbR).ToString() + ", " + Math.Round(colorbG).ToString() + ", " + Math.Round(colorbB).ToString()/* + "\n" + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x10)) + ", " + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x14)) + "\n" + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x18))*/;
            ColorNLabel.Text = Math.Round(colornR).ToString() + ", " + Math.Round(colornG).ToString() + ", " + Math.Round(colornB).ToString()/* + "\n" + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x20)) + ", "+ String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x24)) + "\n" + String.Format("{0:x2}", Gecko.peek(0x12D1F180 + diff + 0x28))*/;

            Color colorA = Color.FromArgb(Convert.ToInt32(coloraR), Convert.ToInt32(coloraG), Convert.ToInt32(coloraB));
            Color colorB = Color.FromArgb(Convert.ToInt32(colorbR), Convert.ToInt32(colorbG), Convert.ToInt32(colorbB));
            Color colorN = Color.FromArgb(Convert.ToInt32(colornR), Convert.ToInt32(colornG), Convert.ToInt32(colornB));

            AlphaShowBox.BackColor = colorA;
            BravoShowBox.BackColor = colorB;
            NeutralShowBox.BackColor = colorN;
        }

        // Setting up Amiibo
        public void PokeAllAmiibo(uint SetAmiibo)
        {
            if (SetAmiibo != 0x69696969)
            {
                Gecko.poke(0x12D1F130, SetAmiibo);
            }
        }
        private void AmiiboPoke_Click(object sender, EventArgs e)
        {
            PokeAllAmiibo(((UintWrapper)AmiiboCBox.SelectedItem).dataVal);

            MessageBox.Show("Success!", "AmiiboTool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public void PokeHedNameAmiibo(string SetHed)
        {
            if (SetHed != "<no change>")
            {
                writeString(0x105E6310, SetHed, "SUP001".Length);
            }
        }

        public void PokeBodyNameAmiibo(string SetClt)
        {
            if (SetClt != "<no change>")
            {
                uint[] amiiboCltAddress = { 0x105E6340, 0x105E6320 };

                for (int num2 = 0; num2 < 2; num2++)
                {
                    writeString(amiiboCltAddress[num2], SetClt, "NoClothes".Length);
                }
            }
        }

        public void PokeShoesNameAmiibo(string SetShs)
        {
            if (SetShs != "<no change>")
            {
                uint[] amiiboShsAddress = { 0x105E6338, 0x105E6318 };

                for (int num2 = 0; num2 < 2; num2++)
                {
                    writeString(amiiboShsAddress[num2], SetShs, "NoShoes".Length);
                }
            }
        }

        private void HedSendButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                PokeHedNameAmiibo(((NameWrapper)HeadCBox.SelectedItem).dataName);
                PokeBodyNameAmiibo(((NameWrapper)BodyCBox.SelectedItem).dataName);
                PokeShoesNameAmiibo(((NameWrapper)ShoesCBox.SelectedItem).dataName);
            }
            catch (NullReferenceException)
            {
                PokeHedNameAmiibo(HeadCBox.Text);
                PokeBodyNameAmiibo(BodyCBox.Text);
                PokeShoesNameAmiibo(ShoesCBox.Text);
            }
            MessageBox.Show("Clothes Patched Successfully!", "AmiiboTool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void SetOriginalClothingAmiiboButton_Click(object sender, EventArgs e)
        {
            writeString(0x105E6310, "NoHed", "SUP001".Length);
            writeString(0x105E6340, "TLY004", "NoClothes".Length);
            writeString(0x105E6338, "SHI003", "NoShoes".Length);
            writeString(0x105E6318, "SHT000", "NoShoes".Length);
            writeString(0x105E6320, "TES000", "NoClothes".Length);

            MessageBox.Show("Clothes set back to original Successfully!", "AmiiboTool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void HighJumpButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EC2E4, 0x41000000);
        }

        private void BigInklingButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EF2B0, 0x3FC00000);
        }

        private void SmallInklingButon_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EF2B0, 0x3F000000);
        }

        private void InvInklingButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EF2B0, 0x30000000);
        }

        private void SpeedHackButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EBE40, 0x3F100000);
        }

        private void NoHUDButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x10650778, 0x48000000);
            Gecko.poke(0x10655644, 0x48000000);
            Gecko.poke(0x106556E0, 0x48000000);
        }

        private void DisSpawnBarrierButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105D7F10, 0x00000000);
        }

        private void InstantSpecialButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EBFA8, 0xBF400000);
        }

        private void StaticAntiBanButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x106E868C, 0x7AAF4CD4);
        }

        private void GhostInklingButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105098DC, 0xF0000000);
        }

        private void SafeOctoHaxButton_Click(object sender, EventArgs e)
        {
            writeString(0x105EF3B0, "Rival00", "Player00".Length);
            writeString(0x105EF3CC, "Rival_Squid", "Player_Squid".Length);
            writeString(0x105EF3BC, "Rival00_Hlf", "Player00_Hlf".Length);
        }

        private void PlazaOverMatchRoom_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x39C96930, 0x645F506C);
            Gecko.poke(0x39C96934, 0x617A6130);
            Gecko.poke(0x39C96938, 0x30000000);
            Gecko.poke(0x106E868C, 0x7AAF4CD4);
        }

        private void WorldOverMchButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x39C96930, 0x645F576F);
            Gecko.poke(0x39C96934, 0x726C6430);
            Gecko.poke(0x39C96938, 0x30000000);
            Gecko.poke(0x106E868C, 0x7AAF4CD4);
        }

        private void GunShopOverMchButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x39C96930, 0x645F526F);
            Gecko.poke(0x39C96934, 0x6F6D5F47);
            Gecko.poke(0x39C96938, 0x65617200);
            Gecko.poke(0x106E868C, 0x7AAF4CD4);
        }

        private void ShoesShopOverMch_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x39C96930, 0x645F526F);
            Gecko.poke(0x39C96934, 0x6F6D5F53);
            Gecko.poke(0x39C96938, 0x686F6573);
            Gecko.poke(0x106E868C, 0x7AAF4CD4);
        }

        private void RevertAllCodesButton_Click(object sender, EventArgs e)
        {
            Gecko.poke(0x105EC2E4, 0x3F933333);
            Gecko.poke(0x105EF2B0, 0x3F800000);
            Gecko.poke(0x105EBE40, 0x3F000000);
            Gecko.poke(0x10650778, 0x3F800000);
            Gecko.poke(0x10655644, 0x3F800000);
            Gecko.poke(0x106556E0, 0x3F800000);
            Gecko.poke(0x105D7F10, 0x42480000);
            Gecko.poke(0x105EBFA8, 0x43300000);
            Gecko.poke(0x105098DC, 0x3F000000);
            if (KeepOctoHaxCheck.Checked)
            {
                Gecko.poke(0x105098DC, 0x3F000000);
            }
            else
            {
                writeString(0x105EF3B0, "Player00", "Player00".Length);
                writeString(0x105EF3CC, "Player_Squid", "Player_Squid".Length);
                writeString(0x105EF3BC, "Player00_Hlf", "Player00_Hlf".Length);
            }
            Gecko.poke(0x39C96930, 0x645F506C);
            Gecko.poke(0x39C96934, 0x617A614C);
            Gecko.poke(0x39C96938, 0x6F626279);
            MessageBox.Show("All Codes Reverted!", "GeckoCodes", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private uint getZAddressForMap()
        {
            try
            {
                return this.Gecko.peek(this.ppbase) + this.ppadd;
            }
            catch (Exception ex)
            {
                return uint.MaxValue;
            }
        }

        private uint getP2ZAddressForMap()
        {
            try
            {
                uint num1 = this.Gecko.peek(this.pp2base);
                int num2 = (int)MessageBox.Show("Operation failed: \n\n" + string.Format("{0:x2}", (object)this.Gecko.peek(this.pp2base)) + "\n\nError reading memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return num1 + this.pp2add;
            }
            catch (Exception ex)
            {
                return uint.MaxValue;
            }
        }

        private void SaveState(int state)
        {
            if ((int)this.Gecko.peek(this.ZAddress) == 0)
            {
                int num1 = (int)MessageBox.Show("Gecko.peek(ZAddress) == 0x00000000!\n\nYou may need to recalculate pointers!", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                try
                {
                    this.val1states[state] = this.Gecko.peek(this.ZAddress - this.val1);
                    this.val2states[state] = this.Gecko.peek(this.ZAddress - this.val2);
                    this.val3states[state] = this.Gecko.peek(this.ZAddress - this.val3);
                    this.val4states[state] = this.Gecko.peek(this.ZAddress - this.val4);
                    this.val5states[state] = this.Gecko.peek(this.ZAddress - this.val5);
                    this.val6states[state] = this.Gecko.peek(this.ZAddress + this.val6);
                    this.val7states[state] = this.Gecko.peek(this.ZAddress);
                    this.EventLogBox.Items.Add((object)("Saved state " + (object)state + ", peeked vals:" + string.Format("{0:x2}", (object)this.val1states[state]) + "," + string.Format("{0:x2}", (object)this.val2states[state]) + "," + string.Format("{0:x2}", (object)this.val3states[state]) + "," + string.Format("{0:x2}", (object)this.val4states[state]) + "," + string.Format("{0:x2}", (object)this.val5states[state]) + "," + string.Format("{0:x2}", (object)this.val6states[state]) + ","));
                }
                catch (Exception ex)
                {
                    int num2 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError reading memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void PokeState(int state)
        {
            if ((int)this.Gecko.peek(this.ZAddress) == 0)
            {
                int num1 = (int)MessageBox.Show("Gecko.peek(ZAddress) == 0x00000000!\n\nYou may need to recalculate pointers!", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                try
                {
                    this.Gecko.poke(this.ZAddress - this.val1, this.val1states[state]);
                    this.Gecko.poke(this.ZAddress - this.val2, this.val2states[state]);
                    this.Gecko.poke(this.ZAddress - this.val3, this.val3states[state]);
                    this.Gecko.poke(this.ZAddress - this.val4, this.val4states[state]);
                    this.Gecko.poke(this.ZAddress - this.val5, this.val5states[state]);
                    this.Gecko.poke(this.ZAddress + this.val6, this.val6states[state]);
                    this.Gecko.poke(this.ZAddress, this.val7states[state]);
                    this.EventLogBox.Items.Add((object)("Poked state " + (object)state + ", poked vals:" + string.Format("{0:x2}", (object)this.val1states[state]) + "," + string.Format("{0:x2}", (object)this.val2states[state]) + "," + string.Format("{0:x2}", (object)this.val3states[state]) + "," + string.Format("{0:x2}", (object)this.val4states[state]) + "," + string.Format("{0:x2}", (object)this.val5states[state]) + "," + string.Format("{0:x2}", (object)this.val6states[state]) + ","));
                }
                catch (Exception ex)
                {
                    int num2 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError writing to memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void State1SaveButton_Click(object sender, EventArgs e)
        {
            this.SaveState(0);
        }

        private void RecalcPointerButton_Click(object sender, EventArgs e)
        {
            this.ZAddress = this.getZAddressForMap();
            this.P2ZAddress = this.getP2ZAddressForMap();
        }

        private void State1LoadButton_Click(object sender, EventArgs e)
        {
            this.PokeState(0);
        }

        private void State2SaveButton_Click(object sender, EventArgs e)
        {
            this.SaveState(1);
        }

        private void State2LoadButton_Click(object sender, EventArgs e)
        {
            this.PokeState(1);
        }

        private void State3SaveButton_Click(object sender, EventArgs e)
        {
            this.SaveState(2);
        }

        private void State3LoadButton_Click(object sender, EventArgs e)
        {
            this.PokeState(2);
        }

        private void ZUpManButton_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.EventLogBox.Items.Add((object)("Getting val for Z: " + string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress))));
                this.textBox1.Text = string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress));
            }
            else
            {
                uint input = this.Gecko.peek(this.ZAddress);
                uint num1 = this.Gecko.peek(this.ZAddress);
                uint num2 = !this.radioButton1.Checked ? (!this.radioButton2.Checked ? (!this.radioButton3.Checked ? (!this.radioButton4.Checked ? this.ChangeUintPos(input, 0, '4') : Convert.ToUInt32(this.textBox1.Text, 16)) : this.ChangeUintPos(this.ChangeUintPos(input, 0, '4'), 1, '4')) : this.ChangeUintPos(this.ChangeUintPos(input, 0, '4'), 1, '3')) : this.ChangeUintPos(this.ChangeUintPos(input, 0, '4'), 1, '1');
                try
                {
                    this.Gecko.poke(this.ZAddress, num2);
                    this.EventLogBox.Items.Add((object)("Poked! VAL: " + string.Format("{0:x2}", (object)num2) + ", PRE: " + string.Format("{0:x2}", (object)num1)));
                }
                catch (Exception ex)
                {
                    int num3 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError reading/writing to memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        public uint ChangeUintPos(uint input, int pos, char tochangeto)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder(string.Format("{0:x2}", (object)input));
                stringBuilder[pos] = tochangeto;
                return Convert.ToUInt32(stringBuilder.ToString(), 16);
            }
            catch (Exception ex)
            {
                return uint.MaxValue;
            }
        }

        private void UpManButton_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.EventLogBox.Items.Add((object)("Getting val for Z: " + string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress - 4U))));
                this.textBox1.Text = string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress - 4U));
            }
            else
            {
                uint input = this.Gecko.peek(this.ZAddress - 4U);
                uint num1 = this.Gecko.peek(this.ZAddress - 4U);
                uint num2 = !this.radioButton1.Checked ? (!this.radioButton2.Checked ? (!this.radioButton3.Checked ? (!this.radioButton4.Checked ? this.ChangeUintPos(input, 0, '4') : Convert.ToUInt32(this.textBox1.Text, 16)) : input + 6291456U) : input + 4194304U) : input + 262144U;
                try
                {
                    this.Gecko.poke(this.ZAddress - this.val5, num2);
                    this.EventLogBox.Items.Add((object)("Poked! VAL: " + string.Format("{0:x2}", (object)num2) + ", PRE: " + string.Format("{0:x2}", (object)num1)));
                }
                catch (Exception ex)
                {
                    int num3 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError reading/writing to memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void HeadCBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BackwardsManButton_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.EventLogBox.Items.Add((object)("Getting val: " + string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress + 4U))));
                this.textBox1.Text = string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress + 4U));
            }
            else
            {
                uint input = this.Gecko.peek(this.ZAddress + 4U);
                uint num1 = this.Gecko.peek(this.ZAddress + 4U);
                uint num2 = !this.radioButton1.Checked ? (!this.radioButton2.Checked ? (!this.radioButton3.Checked ? (!this.radioButton4.Checked ? this.ChangeUintPos(input, 0, '4') : Convert.ToUInt32(this.textBox1.Text, 16)) : input - 6291456U) : input - 4194304U) : input - 262144U;
                try
                {
                    this.Gecko.poke(this.ZAddress + this.val5, num2);
                    this.EventLogBox.Items.Add((object)("Poked! VAL: " + string.Format("{0:x2}", (object)num2) + ", PRE: " + string.Format("{0:x2}", (object)num1)));
                }
                catch (Exception ex)
                {
                    int num3 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError reading/writing to memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.EventLogBox.Items.Add((object)("Getting val: " + string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress - 4U))));
                this.textBox1.Text = string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress - 4U));
            }
            else
            {
                uint input = this.Gecko.peek(this.ZAddress - 4U);
                uint num1 = this.Gecko.peek(this.ZAddress - 4U);
                uint num2 = !this.radioButton1.Checked ? (!this.radioButton2.Checked ? (!this.radioButton3.Checked ? (!this.radioButton4.Checked ? this.ChangeUintPos(input, 0, '4') : Convert.ToUInt32(this.textBox1.Text, 16)) : input - 6291456U) : input - 4194304U) : input - 262144U;
                try
                {
                    this.Gecko.poke(this.ZAddress - this.val5, num2);
                    this.EventLogBox.Items.Add((object)("Poked! VAL: " + string.Format("{0:x2}", (object)num2) + ", PRE: " + string.Format("{0:x2}", (object)num1)));
                }
                catch (Exception ex)
                {
                    int num3 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError reading/writing to memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void ForwardManButton_Click(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.EventLogBox.Items.Add((object)("Getting val: " + string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress + 4U))));
                this.textBox1.Text = string.Format("{0:x2}", (object)this.Gecko.peek(this.ZAddress + 4U));
            }
            else
            {
                uint input = this.Gecko.peek(this.ZAddress + 4U);
                uint num1 = this.Gecko.peek(this.ZAddress + 4U);
                uint num2 = !this.radioButton1.Checked ? (!this.radioButton2.Checked ? (!this.radioButton3.Checked ? (!this.radioButton4.Checked ? this.ChangeUintPos(input, 0, '4') : Convert.ToUInt32(this.textBox1.Text, 16)) : input + 6291456U) : input + 4194304U) : input + 262144U;
                try
                {
                    this.Gecko.poke(this.ZAddress + this.val5, num2);
                    this.EventLogBox.Items.Add((object)("Poked! VAL: " + string.Format("{0:x2}", (object)num2) + ", PRE: " + string.Format("{0:x2}", (object)num1)));
                }
                catch (Exception ex)
                {
                    int num3 = (int)MessageBox.Show("Operation failed: \n\n" + (object)ex + "\n\nError reading/writing to memory.", "GeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox1.Checked)
            {
                this.radioButton1.Enabled = false;
                this.radioButton2.Enabled = false;
                this.radioButton3.Enabled = false;
                this.radioButton4.Enabled = false;
                this.textBox1.ReadOnly = true;
            }
            else
            {
                this.radioButton1.Enabled = true;
                this.radioButton2.Enabled = true;
                this.radioButton3.Enabled = true;
                this.radioButton4.Enabled = true;
                this.textBox1.ReadOnly = false;
            }
        }

        private void SpecialWepPokeButton_Click(object sender, EventArgs e)
        {
            PokeMainWep(((UintWrapper)MainWepCBox.SelectedItem).dataVal);
            PokeSubWep(((UintWrapper)SubWepCBox.SelectedItem).dataVal);
            PokeSpecialWep(((UintWrapper)SpecialWepCBox.SelectedItem).dataVal);
            MessageBox.Show("Weapon Settings have been set correctly!", "JGeckoTool", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void SetAlphaTeamButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x2C, 0x00000000);
        }

        private void SetBravoTeamButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x2C, 0x00000001);
        }

        private void RMWeaponButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x704, 0xFFFFFFFF);
            Gecko.poke(point + 0x708, 0x01014992);
        }

        public void PokeMainWep(uint SetMnWep)
        {
            if (SetMnWep != 0x69696969)
            {
                uint point = Gecko.peek(0x106E46E8);
                Gecko.poke(point + 0x78, SetMnWep);
            }
        }

        public void PokeSubWep(uint SetSbWep)
        {
            if (SetSbWep != 0x69696969)
            {
                uint point = Gecko.peek(0x106E46E8);
                Gecko.poke(point + 0x7C, SetSbWep);
            }
        }

        private void TabPage9_Click(object sender, EventArgs e)
        {

        }

        public void PokeSpecialWep(uint SetSpWep)
        {
            if (SetSpWep != 0x69696969)
            {
                uint point = Gecko.peek(0x106E46E8);
                Gecko.poke(point + 0x80, SetSpWep);
            }
        }

        private void GetSpecialButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x7FC, 0x0FFFFFFF);
        }

        private void InviciblePlayerButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x580, 0x00000002);
        }

        private void BotTPButton1_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x13CC, YouX);
            Gecko.poke(point + 0x13D0, YouY);
            Gecko.poke(point + 0x13D4, YouZ);
            Gecko.poke(point + 0x2550, YouX);
            Gecko.poke(point + 0x2554, YouY);
            Gecko.poke(point + 0x2558, YouZ);
            Gecko.poke(point + 0x36D4, YouX);
            Gecko.poke(point + 0x36D8, YouY);
            Gecko.poke(point + 0x36DC, YouZ);
            Gecko.poke(point + 0x4858, YouX);
            Gecko.poke(point + 0x485C, YouY);
            Gecko.poke(point + 0x4860, YouZ);
            Gecko.poke(point + 0x59DC, YouX);
            Gecko.poke(point + 0x59E0, YouY);
            Gecko.poke(point + 0x59E4, YouZ);
            Gecko.poke(point + 0x6B60, YouX);
            Gecko.poke(point + 0x6B64, YouY);
            Gecko.poke(point + 0x6B68, YouZ);
            Gecko.poke(point + 0x7CE4, YouX);
            Gecko.poke(point + 0x7CE8, YouY);
            Gecko.poke(point + 0x7CEC, YouZ);
        }

        private void SaveCoords(int Coord)
        {
            uint point = Gecko.peek(0x106E46E8);
            this.val1states[Coord] = this.Gecko.peek(point + 0x248);
            this.val2states[Coord] = this.Gecko.peek(point + 0x24C);
            this.val3states[Coord] = this.Gecko.peek(point + 0x250);
        }

        private void WoomyLoadCoords(int Coord)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x6F4, 0x00000001);
            Gecko.poke(point + 0x6A4, this.val1states[Coord]);
            Gecko.poke(point + 0x6A8, this.val2states[Coord]);
            Gecko.poke(point + 0x6AC, this.val3states[Coord]);
        }

        private void SaveCoords1Button_Click(object sender, EventArgs e)
        {
            this.SaveCoords(0);
        }

        private void LoadWoomyCoords1Button_Click(object sender, EventArgs e)
        {
            this.WoomyLoadCoords(0);
        }

        private void Lv3InkTankButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x88, 0x00000004);
        }

        private void PlayerSuicideButton_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            Gecko.poke(point + 0x4E0, 0x000000C0);
        }

        public void PokeConstantJGeckoCodes()
        {
            stopBtnClk = false;

            startBtnClk = true;

            while (true)
            {
                //some code to be executed
                uint point = Gecko.peek(0x106E46E8);
                uint point1 = Gecko.peek(point + 0x760);
                if (MoonJumpCheckBox.Checked)
                {
                    Gecko.poke(point + 0x5C8, 0x00000000);
                    Gecko.poke(point + 0x5D8, 0x00000000);
                }
                if (SwimEverywhereCheckBox.Checked)
                {
                    Gecko.poke(point + 0x88C, 0x43000000);
                }
                if (InfInkCheckBox.Checked)
                {
                    Gecko.poke(point1 + 0x1C, 0x3F800000);
                }
                if (FastRespawnCheckBox.Checked)
                {
                    Gecko.poke(point + 0x4E4, 0x00000000);
                    Gecko.poke(point + 0x4E8, 0x00000000);
                    Gecko.poke(point + 0x4EC, 0x00000000);
                }
                if (RapidSJCheckBox.Checked)
                {
                    Gecko.poke(point + 0x6F8, 0x00000000);
                }
                if (InvinciblePLayerCheckbox.Checked)
                {
                    Gecko.poke(point + 0x580, 0x00000002);
                }
                if (stopBtnClk == true)
                {
                    break;
                }
            }
        }

        private void SendJGeckoCodesButton_Click(object sender, EventArgs e)
        {
            ThreadStart childref = new ThreadStart(PokeConstantJGeckoCodes);
            Thread childThread = new Thread(childref);
            childThread.Start();
            BreakJGeckoLoopButton.Enabled = true;
            SendJGeckoCodesButton.Enabled = false;
        }

        private void BreakJGeckoLoopButton_Click(object sender, EventArgs e)
        {
            stopBtnClk = true;
            BreakJGeckoLoopButton.Enabled = false;
            SendJGeckoCodesButton.Enabled = true;
        }

        private void P1Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P2Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x13CC);
            uint YouY = Gecko.peek(point + 0x13D0);
            uint YouZ = Gecko.peek(point + 0x13D4);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P3Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x2550);
            uint YouY = Gecko.peek(point + 0x2554);
            uint YouZ = Gecko.peek(point + 0x2558);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P4Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x36D4);
            uint YouY = Gecko.peek(point + 0x36D8);
            uint YouZ = Gecko.peek(point + 0x36DC);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P5Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x4858);
            uint YouY = Gecko.peek(point + 0x485C);
            uint YouZ = Gecko.peek(point + 0x4860);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P6Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x59DC);
            uint YouY = Gecko.peek(point + 0x59E0);
            uint YouZ = Gecko.peek(point + 0x59E4);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P7Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x6B60);
            uint YouY = Gecko.peek(point + 0x6B64);
            uint YouZ = Gecko.peek(point + 0x6B68);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P8Stalking_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x7CE4);
            uint YouY = Gecko.peek(point + 0x7CE8);
            uint YouZ = Gecko.peek(point + 0x7CEC);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P1Dummy_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x248, YouX);
            Gecko.poke(point + 0x24C, YouY);
            Gecko.poke(point + 0x250, YouZ);
        }

        private void P2Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x13CC, YouX);
            Gecko.poke(point + 0x13D0, YouY);
            Gecko.poke(point + 0x13D4, YouZ);
        }

        private void P3Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x2550, YouX);
            Gecko.poke(point + 0x2554, YouY);
            Gecko.poke(point + 0x2558, YouZ);
        }

        private void P4Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x36D4, YouX);
            Gecko.poke(point + 0x36D8, YouY);
            Gecko.poke(point + 0x36DC, YouZ);
        }

        private void P5Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x4858, YouX);
            Gecko.poke(point + 0x485C, YouY);
            Gecko.poke(point + 0x4860, YouZ);
        }

        private void P6Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x59DC, YouX);
            Gecko.poke(point + 0x59E0, YouY);
            Gecko.poke(point + 0x59E4, YouZ);
        }

        private void P7Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x6B60, YouX);
            Gecko.poke(point + 0x6B64, YouY);
            Gecko.poke(point + 0x6B68, YouZ);
        }

        private void P8Get2Coords_Click(object sender, EventArgs e)
        {
            uint point = Gecko.peek(0x106E46E8);
            uint YouX = Gecko.peek(point + 0x248);
            uint YouY = Gecko.peek(point + 0x24C);
            uint YouZ = Gecko.peek(point + 0x250);
            Gecko.poke(point + 0x7CE4, YouX);
            Gecko.poke(point + 0x7CE8, YouY);
            Gecko.poke(point + 0x7CEC, YouZ);
        }
    }
}
