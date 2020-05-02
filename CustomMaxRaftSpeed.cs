namespace CustomMaxSpeed
{
    using JetBrains.Annotations;

    [ModTitle("CustomMaxRaftSpeed")]
    [ModDescription("Gives you the ability to customize the maximum raft speed.")]
    [ModAuthor("janniksam")]
    [ModIconUrl("https://raw.githubusercontent.com/janniksam/RaftMod.CustomMaxRaftSpeed/master/banner.png")]
    [ModWallpaperUrl("https://raw.githubusercontent.com/janniksam/RaftMod.CustomMaxRaftSpeed/master/banner.png")]
    [ModVersionCheckUrl("https://www.raftmodding.com/api/v1/mods/custommaxraftspeed/version.txt")]
    [ModVersion("1.0")]
    [RaftVersion("Update 11 (4677160)")]
    [ModIsPermanent(false)]
    public class CustomMaxRaftSpeed : Mod
    {
        private const string ModNamePrefix = "<color=#42a7f5>Filtered</color><color=#FF0000>Nets</color>";
        private const float DefaultMaxSpeed = 1.5f;
        private const float DefaultWaterDriftSpeed = 1.5f;
        private const string MaxSpeedArgumentIsOutOfRangeMessage = "maxspeed: Requires a factor between 1 and 20. (x times of the default speed)\n" +
                                                                   "e.g. \"maxspeed 1.5\" will increase your speed to 150%, while \"maxspeed 20\" will increase your speed to 2000%)";

        private float m_maxSpeedFactor = DefaultMaxSpeed;
        private float m_waterDriftSpeedFactor = DefaultWaterDriftSpeed;
        
        [UsedImplicitly]
        public void Start()
        {
            RConsole.Log(string.Format("{0} has been loaded!", ModNamePrefix));
            RConsole.registerCommand(typeof(CustomMaxRaftSpeed),
                "Modify the maximal speed of your raft",
                "maxspeed",
                SetMaximumRaftSpeed);
        }

        [UsedImplicitly]
        public void OnModUnload()
        {
            var raft = ComponentManager<Raft>.Value;
            if (raft == null)
            {
                return;
            }

            raft.maxSpeed = DefaultMaxSpeed;
            raft.waterDriftSpeed = DefaultWaterDriftSpeed;

            RConsole.Log(string.Format("{0} has been unloaded!", ModNamePrefix));
            Destroy(gameObject);
        }

        public void Update()
        {
            if (Semih_Network.InLobbyScene ||
                !Semih_Network.IsHost)
            {
                return;
            }

            var raft = ComponentManager<Raft>.Value;
            if (raft == null)
            {
                return;
            }

            raft.maxSpeed = DefaultMaxSpeed * m_maxSpeedFactor;
            raft.waterDriftSpeed = DefaultWaterDriftSpeed * m_waterDriftSpeedFactor;
        }

        private void SetMaximumRaftSpeed()
        {
            var args = RConsole.lcargs;
            if (args.Length != 2)
            {
                RConsole.Log(MaxSpeedArgumentIsOutOfRangeMessage);
                return;
            }

            float maxSpeedFloat = -1f;
            int maxSpeedInt = -1;
            if ((!float.TryParse(args[1], out maxSpeedFloat) && !int.TryParse(args[1], out maxSpeedInt)))
            {
                RConsole.Log(MaxSpeedArgumentIsOutOfRangeMessage);
                return;
            }

            if (maxSpeedFloat < 0f)
            {
                maxSpeedFloat = maxSpeedInt;
            }

            if (maxSpeedFloat < 1f || maxSpeedFloat > 20f)
            {
                RConsole.Log(MaxSpeedArgumentIsOutOfRangeMessage);
                return;
            }
            
            m_maxSpeedFactor = maxSpeedFloat;
            m_waterDriftSpeedFactor = maxSpeedFloat;
            RConsole.Log(string.Format("{0}: Set max speed factor set to {1}", ModNamePrefix, maxSpeedFloat));
        }
    }
}
