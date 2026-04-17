using System.IO;

namespace CupheadOnline.Net
{
    // ──────────────────────────────────────────────────────────────────────────
    //  Packet serialization — uses System.IO.BinaryWriter/BinaryReader
    //  (available in .NET 2.0+ / Mono, no external dependencies)
    // ──────────────────────────────────────────────────────────────────────────

    public interface IPacket
    {
        void Write(BinaryWriter w);
        void Read(BinaryReader r);
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  Packet type discriminator — fits in one byte on the wire
    //  (bit 7 is reserved by NetManager for the reliable flag)
    // ──────────────────────────────────────────────────────────────────────────
    public enum PacketType : byte
    {
        PlayerState     = 0,   // unreliable-sequenced, every FixedUpdate
        InputFrame      = 1,   // unreliable-sequenced, every FixedUpdate
        WeaponEvent     = 2,   // reliable-ordered
        DamageEvent     = 3,   // reliable-ordered
        EnemyState      = 4,   // unreliable-sequenced, 20 Hz
        SceneChange     = 5,   // reliable-ordered
        LobbySync       = 6,   // reliable-ordered (loadout exchange)
        Ping            = 7,   // unreliable
        Pong            = 8,   // unreliable
        SessionStart    = 9,   // reliable — host→client once connected
        Disconnect      = 10,  // reliable — graceful goodbye
        // ── Handshake (no payload) ─────────────────────────────────────────
        Hello           = 11,  // client → host: "I'm here"
        Welcome         = 12,  // host  → client: "Accepted, send Ready"
        Ready           = 13,  // client → host: "Let's play"
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  Per-packet structs
    // ──────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Sent every FixedUpdate (60 Hz) on the unreliable channel.
    /// Host sends P1 state; client sends P2 state.
    /// </summary>
    public struct PlayerStatePacket : IPacket
    {
        public byte   PlayerId;   // 0=PlayerOne, 1=PlayerTwo
        public float  PosX;
        public float  PosY;
        public sbyte  LookX;     // Trilean.Value: -1 / 0 / 1
        public sbyte  LookY;
        public byte   Flags;     // bit0=Grounded bit1=Dashing bit2=Ducking bit3=GravReversed bit4=IsHit bit5=IsUsingSuperOrEx
        public byte   AnimState; // lower 8 bits of animator state hash (best-effort)
        public uint   Tick;

        public bool Grounded     => (Flags & 1)  != 0;
        public bool Dashing      => (Flags & 2)  != 0;
        public bool Ducking      => (Flags & 4)  != 0;
        public bool GravReversed => (Flags & 8)  != 0;
        public bool IsHit        => (Flags & 16) != 0;
        public bool IsSuper      => (Flags & 32) != 0;

        public void Write(BinaryWriter w)
        {
            w.Write(PlayerId);
            w.Write(PosX);
            w.Write(PosY);
            w.Write(LookX);
            w.Write(LookY);
            w.Write(Flags);
            w.Write(AnimState);
            w.Write(Tick);
        }

        public void Read(BinaryReader r)
        {
            PlayerId  = r.ReadByte();
            PosX      = r.ReadSingle();
            PosY      = r.ReadSingle();
            LookX     = r.ReadSByte();
            LookY     = r.ReadSByte();
            Flags     = r.ReadByte();
            AnimState = r.ReadByte();
            Tick      = r.ReadUInt32();
        }
    }

    /// <summary>
    /// Client → Host every FixedUpdate: raw input state.
    /// Host uses these to drive PlayerTwo's motor so simulation runs identically.
    /// </summary>
    public struct InputFramePacket : IPacket
    {
        public float  AxisX;
        public float  AxisY;
        public uint   Buttons;  // bit-field: bit-index = (int)CupheadButton
        public uint   Tick;

        public bool IsPressed(CupheadButton btn) => (Buttons & (1u << (int)btn)) != 0;

        public void Write(BinaryWriter w)
        {
            w.Write(AxisX);
            w.Write(AxisY);
            w.Write(Buttons);
            w.Write(Tick);
        }

        public void Read(BinaryReader r)
        {
            AxisX   = r.ReadSingle();
            AxisY   = r.ReadSingle();
            Buttons = r.ReadUInt32();
            Tick    = r.ReadUInt32();
        }
    }

    /// <summary>
    /// Reliable: a discrete weapon event (shot, EX, super, parry).
    /// </summary>
    public struct WeaponEventPacket : IPacket
    {
        public byte  PlayerId;
        public byte  EventType; // 0=BasicShot 1=Ex 2=Super 3=Parry 4=WeaponSwitch
        public sbyte AimX;     // Trilean look direction at time of fire
        public sbyte AimY;
        public byte  WeaponId; // (int)Weapon enum – for switch events
        public uint  Tick;

        public void Write(BinaryWriter w)
        {
            w.Write(PlayerId);
            w.Write(EventType);
            w.Write(AimX);
            w.Write(AimY);
            w.Write(WeaponId);
            w.Write(Tick);
        }

        public void Read(BinaryReader r)
        {
            PlayerId  = r.ReadByte();
            EventType = r.ReadByte();
            AimX      = r.ReadSByte();
            AimY      = r.ReadSByte();
            WeaponId  = r.ReadByte();
            Tick      = r.ReadUInt32();
        }
    }

    /// <summary>
    /// Reliable, host→client: authoritative damage confirmation.
    /// </summary>
    public struct DamageEventPacket : IPacket
    {
        public byte  TargetPlayerId;
        public float Damage;
        public byte  Source;   // (byte)DamageDealer.DamageSource
        public uint  Tick;

        public void Write(BinaryWriter w)
        {
            w.Write(TargetPlayerId);
            w.Write(Damage);
            w.Write(Source);
            w.Write(Tick);
        }

        public void Read(BinaryReader r)
        {
            TargetPlayerId = r.ReadByte();
            Damage         = r.ReadSingle();
            Source         = r.ReadByte();
            Tick           = r.ReadUInt32();
        }
    }

    /// <summary>
    /// Unreliable, host→client: boss / enemy position + phase.
    /// </summary>
    public struct EnemyStatePacket : IPacket
    {
        public int   InstanceId; // UnityEngine.Object.GetInstanceID()
        public float PosX;
        public float PosY;
        public float Hp;
        public byte  Phase;
        public int   AnimHash;   // Animator.GetCurrentAnimatorStateInfo(0).fullPathHash
        public uint  Tick;

        public void Write(BinaryWriter w)
        {
            w.Write(InstanceId);
            w.Write(PosX);
            w.Write(PosY);
            w.Write(Hp);
            w.Write(Phase);
            w.Write(AnimHash);
            w.Write(Tick);
        }

        public void Read(BinaryReader r)
        {
            InstanceId = r.ReadInt32();
            PosX       = r.ReadSingle();
            PosY       = r.ReadSingle();
            Hp         = r.ReadSingle();
            Phase      = r.ReadByte();
            AnimHash   = r.ReadInt32();
            Tick       = r.ReadUInt32();
        }
    }

    /// <summary>
    /// Reliable, host→client: trigger a level/scene transition and share the RNG seed.
    /// </summary>
    public struct SceneChangePacket : IPacket
    {
        public int  LevelEnum;  // (int)Levels enum value
        public uint RngSeed;

        public void Write(BinaryWriter w)
        {
            w.Write(LevelEnum);
            w.Write(RngSeed);
        }

        public void Read(BinaryReader r)
        {
            LevelEnum = r.ReadInt32();
            RngSeed   = r.ReadUInt32();
        }
    }

    /// <summary>
    /// Reliable, bidirectional: exchange character + loadout before the level loads.
    /// </summary>
    public struct LobbySyncPacket : IPacket
    {
        public byte PlayerId;
        public byte Weapon1;    // (byte)Weapon enum
        public byte Weapon2;
        public byte Super;      // (byte)Super enum
        public byte Charm;      // (byte)Charm enum
        public byte IsChalice;  // 1 = playing as Ms. Chalice

        public void Write(BinaryWriter w)
        {
            w.Write(PlayerId);
            w.Write(Weapon1);
            w.Write(Weapon2);
            w.Write(Super);
            w.Write(Charm);
            w.Write(IsChalice);
        }

        public void Read(BinaryReader r)
        {
            PlayerId  = r.ReadByte();
            Weapon1   = r.ReadByte();
            Weapon2   = r.ReadByte();
            Super     = r.ReadByte();
            Charm     = r.ReadByte();
            IsChalice = r.ReadByte();
        }
    }

    /// <summary>
    /// Reliable: sent by host once connection is confirmed.
    /// </summary>
    public struct SessionStartPacket : IPacket
    {
        public int  CurrentLevel;
        public uint CurrentTick;
        public uint RngSeed;

        public void Write(BinaryWriter w)
        {
            w.Write(CurrentLevel);
            w.Write(CurrentTick);
            w.Write(RngSeed);
        }

        public void Read(BinaryReader r)
        {
            CurrentLevel = r.ReadInt32();
            CurrentTick  = r.ReadUInt32();
            RngSeed      = r.ReadUInt32();
        }
    }
}
