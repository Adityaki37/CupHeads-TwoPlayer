Imports System
Imports UnityEngine

' Token: 0x02000431 RID: 1073
<Serializable()>
Public Class EnemyProperties
	' Token: 0x06000FA8 RID: 4008 RVA: 0x0009C6AC File Offset: 0x0009AAAC
	Public Sub New()
		Me._id = TimeUtils.GetCurrentSecond()
	End Sub

	' Token: 0x1700026C RID: 620
	' (get) Token: 0x06000FA9 RID: 4009 RVA: 0x0009C812 File Offset: 0x0009AC12
	Public ReadOnly Property DisplayName As String
		Get
			Return Me._displayName
		End Get
	End Property

	' Token: 0x1700026D RID: 621
	' (get) Token: 0x06000FAA RID: 4010 RVA: 0x0009C81A File Offset: 0x0009AC1A
	Public ReadOnly Property EnumName As String
		Get
			Return Me._enumName
		End Get
	End Property

	' Token: 0x1700026E RID: 622
	' (get) Token: 0x06000FAB RID: 4011 RVA: 0x0009C822 File Offset: 0x0009AC22
	Public ReadOnly Property ID As Integer
		Get
			Return Me._id
		End Get
	End Property

	' Token: 0x1700026F RID: 623
	' (get) Token: 0x06000FAC RID: 4012 RVA: 0x0009C82A File Offset: 0x0009AC2A
	Public ReadOnly Property Health As Single
		Get
			Return Me._health
		End Get
	End Property

	' Token: 0x17000270 RID: 624
	' (get) Token: 0x06000FAD RID: 4013 RVA: 0x0009C832 File Offset: 0x0009AC32
	Public ReadOnly Property CanParry As Boolean
		Get
			Return Me._parryable
		End Get
	End Property

	' Token: 0x17000271 RID: 625
	' (get) Token: 0x06000FAE RID: 4014 RVA: 0x0009C83A File Offset: 0x0009AC3A
	Public ReadOnly Property MoveLoopMode As EnemyProperties.LoopMode
		Get
			Return Me._moveLoopMode
		End Get
	End Property

	' Token: 0x17000272 RID: 626
	' (get) Token: 0x06000FAF RID: 4015 RVA: 0x0009C842 File Offset: 0x0009AC42
	Public ReadOnly Property MoveSpeed As Single
		Get
			Return Me._moveSpeed
		End Get
	End Property

	' Token: 0x17000273 RID: 627
	' (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0009C84A File Offset: 0x0009AC4A
	Public ReadOnly Property canJump As Boolean
		Get
			Return Me._canJump
		End Get
	End Property

	' Token: 0x17000274 RID: 628
	' (get) Token: 0x06000FB1 RID: 4017 RVA: 0x0009C852 File Offset: 0x0009AC52
	Public ReadOnly Property gravity As Single
		Get
			Return Me._gravity
		End Get
	End Property

	' Token: 0x17000275 RID: 629
	' (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0009C85A File Offset: 0x0009AC5A
	Public ReadOnly Property floatSpeed As Single
		Get
			Return Me._floatSpeed
		End Get
	End Property

	' Token: 0x17000276 RID: 630
	' (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0009C862 File Offset: 0x0009AC62
	Public ReadOnly Property jumpHeight As Single
		Get
			Return Me._jumpHeight
		End Get
	End Property

	' Token: 0x17000277 RID: 631
	' (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0009C86A File Offset: 0x0009AC6A
	Public ReadOnly Property jumpLength As Single
		Get
			Return Me._jumpLength
		End Get
	End Property

	' Token: 0x17000278 RID: 632
	' (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0009C872 File Offset: 0x0009AC72
	Public ReadOnly Property ProjectileAimMode As EnemyProperties.AimMode
		Get
			Return Me._projectileAimMode
		End Get
	End Property

	' Token: 0x17000279 RID: 633
	' (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0009C87A File Offset: 0x0009AC7A
	Public ReadOnly Property ProjectileParryable As Boolean
		Get
			Return Me._projectileParryable
		End Get
	End Property

	' Token: 0x1700027A RID: 634
	' (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0009C882 File Offset: 0x0009AC82
	Public ReadOnly Property ProjectileSpeed As Single
		Get
			Return Me._projectileSpeed
		End Get
	End Property

	' Token: 0x1700027B RID: 635
	' (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0009C88A File Offset: 0x0009AC8A
	Public ReadOnly Property ArcProjectileMinSpeed As Single
		Get
			Return Me._arcProjectileMinSpeed
		End Get
	End Property

	' Token: 0x1700027C RID: 636
	' (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0009C892 File Offset: 0x0009AC92
	Public ReadOnly Property ProjectileAngle As Single
		Get
			Return Me._projectileAngle
		End Get
	End Property

	' Token: 0x1700027D RID: 637
	' (get) Token: 0x06000FBA RID: 4026 RVA: 0x0009C89A File Offset: 0x0009AC9A
	Public ReadOnly Property ArcProjectileMinAngle As Single
		Get
			Return Me._arcProjectileMinAngle
		End Get
	End Property

	' Token: 0x1700027E RID: 638
	' (get) Token: 0x06000FBB RID: 4027 RVA: 0x0009C8A2 File Offset: 0x0009ACA2
	Public ReadOnly Property ProjectileGravity As Single
		Get
			Return Me._projectileGravity
		End Get
	End Property

	' Token: 0x1700027F RID: 639
	' (get) Token: 0x06000FBC RID: 4028 RVA: 0x0009C8AA File Offset: 0x0009ACAA
	Public ReadOnly Property ProjectileStoneTime As Single
		Get
			Return Me._projectileStoneTime
		End Get
	End Property

	' Token: 0x17000280 RID: 640
	' (get) Token: 0x06000FBD RID: 4029 RVA: 0x0009C8B2 File Offset: 0x0009ACB2
	Public ReadOnly Property ProjectileDelay As MinMax
		Get
			Return Me._projectileDelay
		End Get
	End Property

	' Token: 0x17000281 RID: 641
	' (get) Token: 0x06000FBE RID: 4030 RVA: 0x0009C8BA File Offset: 0x0009ACBA
	Public ReadOnly Property MushroomPinkNumber As MinMax
		Get
			Return Me._mushroomPinkNumber
		End Get
	End Property

	' Token: 0x17000282 RID: 642
	' (get) Token: 0x06000FBF RID: 4031 RVA: 0x0009C8C2 File Offset: 0x0009ACC2
	Public ReadOnly Property AcornFlySpeed As Single
		Get
			Return Me._acornFlySpeed
		End Get
	End Property

	' Token: 0x17000283 RID: 643
	' (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0009C8CA File Offset: 0x0009ACCA
	Public ReadOnly Property AcornDropSpeed As Single
		Get
			Return Me._acornDropSpeed
		End Get
	End Property

	' Token: 0x17000284 RID: 644
	' (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0009C8D2 File Offset: 0x0009ACD2
	Public ReadOnly Property AcornPropellerSpeed As Single
		Get
			Return Me._acornPropellerSpeed
		End Get
	End Property

	' Token: 0x17000285 RID: 645
	' (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0009C8DA File Offset: 0x0009ACDA
	Public ReadOnly Property BlobRunnerMeltDelay As MinMax
		Get
			Return Me._blobRunnerMeltDelay
		End Get
	End Property

	' Token: 0x17000286 RID: 646
	' (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0009C8E2 File Offset: 0x0009ACE2
	Public ReadOnly Property BlobRunnerUnmeltLoopTime As Single
		Get
			Return Me._blobRunnerUnnmeltLoopTime
		End Get
	End Property

	' Token: 0x040018DA RID: 6362
	<SerializeField()>
	Private _displayName As String = String.Empty

	' Token: 0x040018DB RID: 6363
	<SerializeField()>
	Private _enumName As String = String.Empty

	' Token: 0x040018DC RID: 6364
	<SerializeField()>
	Private _id As Integer

	' Token: 0x040018DD RID: 6365
	<SerializeField()>
	Private _health As Single = 1F

	' Token: 0x040018DE RID: 6366
	<SerializeField()>
	Private _parryable As Boolean

	' Token: 0x040018DF RID: 6367
	<Header("Movement")>
	<SerializeField()>
	Private _moveLoopMode As EnemyProperties.LoopMode

	' Token: 0x040018E0 RID: 6368
	<SerializeField()>
	Private _moveSpeed As Single = 500F

	' Token: 0x040018E1 RID: 6369
	<SerializeField()>
	Private _canJump As Boolean

	' Token: 0x040018E2 RID: 6370
	<SerializeField()>
	Private _gravity As Single = 1200F

	' Token: 0x040018E3 RID: 6371
	<SerializeField()>
	Private _floatSpeed As Single = 400F

	' Token: 0x040018E4 RID: 6372
	<SerializeField()>
	Private _jumpHeight As Single = 200F

	' Token: 0x040018E5 RID: 6373
	<SerializeField()>
	Private _jumpLength As Single = 500F

	' Token: 0x040018E6 RID: 6374
	<Header("Projectiles")>
	<SerializeField()>
	Private _projectileAimMode As EnemyProperties.AimMode = EnemyProperties.AimMode.Straight

	' Token: 0x040018E7 RID: 6375
	<SerializeField()>
	Private _projectileParryable As Boolean

	' Token: 0x040018E8 RID: 6376
	<SerializeField()>
	Private _projectileSpeed As Single = 500F

	' Token: 0x040018E9 RID: 6377
	<SerializeField()>
	Private _arcProjectileMinSpeed As Single = 250F

	' Token: 0x040018EA RID: 6378
	<SerializeField()>
	Private _projectileAngle As Single

	' Token: 0x040018EB RID: 6379
	<SerializeField()>
	Private _arcProjectileMinAngle As Single

	' Token: 0x040018EC RID: 6380
	<SerializeField()>
	Private _projectileGravity As Single = 15F

	' Token: 0x040018ED RID: 6381
	<SerializeField()>
	Private _projectileStoneTime As Single

	' Token: 0x040018EE RID: 6382
	<SerializeField()>
	Private _projectileDelay As MinMax = New MinMax(1F, 1F)

	' Token: 0x040018EF RID: 6383
	<SerializeField()>
	Private _mushroomPinkNumber As MinMax = New MinMax(3F, 5F)

	' Token: 0x040018F0 RID: 6384
	<SerializeField()>
	Private _acornFlySpeed As Single = 500F

	' Token: 0x040018F1 RID: 6385
	<SerializeField()>
	Private _acornDropSpeed As Single = 500F

	' Token: 0x040018F2 RID: 6386
	<SerializeField()>
	Private _acornPropellerSpeed As Single = 300F

	' Token: 0x040018F3 RID: 6387
	<SerializeField()>
	Private _blobRunnerMeltDelay As MinMax = New MinMax(2F, 3F)

	' Token: 0x040018F4 RID: 6388
	<SerializeField()>
	Private _blobRunnerUnnmeltLoopTime As Single = 0.5F

	' Token: 0x040018F5 RID: 6389
	Public ClamTimeSpeedUp As Single = 0.8F

	' Token: 0x040018F6 RID: 6390
	Public ClamTimeSpeedDown As Single = 1F

	' Token: 0x040018F7 RID: 6391
	Public ClamMaxPointRange As MinMax = New MinMax(600F, 700F)

	' Token: 0x040018F8 RID: 6392
	Public ClamShotCount As Integer = 4

	' Token: 0x040018F9 RID: 6393
	Public ClamDespawnDelayRange As MinMax = New MinMax(3.5F, 5F)

	' Token: 0x040018FA RID: 6394
	Public fastMovement As Single = 400F

	' Token: 0x040018FB RID: 6395
	Public slowMovement As Single = 200F

	' Token: 0x040018FC RID: 6396
	Public dragonFlyAimString As String

	' Token: 0x040018FD RID: 6397
	Public dragonFlyAtkDelayString As String

	' Token: 0x040018FE RID: 6398
	Public dragonFlyWarningDuration As Single

	' Token: 0x040018FF RID: 6399
	Public dragonFlyAttackDuration As Single

	' Token: 0x04001900 RID: 6400
	Public dragonFlyProjectileSpeed As Single

	' Token: 0x04001901 RID: 6401
	Public dragonFlyProjectileDelay As Single

	' Token: 0x04001902 RID: 6402
	Public dragonFlyLockDistOffset As Single

	' Token: 0x04001903 RID: 6403
	Public dragonFlyInitRiseTime As Single

	' Token: 0x04001904 RID: 6404
	Public WoodpeckerWarningDuration As Single

	' Token: 0x04001905 RID: 6405
	Public WoodpeckerAttackDuration As Single

	' Token: 0x04001906 RID: 6406
	Public WoodpeckermoveDownTime As Single

	' Token: 0x04001907 RID: 6407
	Public WoodpeckermoveUpTime As Single

	' Token: 0x04001908 RID: 6408
	Public flyingFishVelocity As Single

	' Token: 0x04001909 RID: 6409
	Public flyingFishSinVelocity As Single

	' Token: 0x0400190A RID: 6410
	Public flyingFishSinSize As Single

	' Token: 0x0400190B RID: 6411
	Public lobsterTuckTime As Single

	' Token: 0x0400190C RID: 6412
	Public lobsterOffscreenTime As Single

	' Token: 0x0400190D RID: 6413
	Public lobsterSpeed As Single

	' Token: 0x0400190E RID: 6414
	Public lobsterWarningTime As Single

	' Token: 0x0400190F RID: 6415
	Public lobsterY As Single

	' Token: 0x04001910 RID: 6416
	Public krillVelocityX As MinMax

	' Token: 0x04001911 RID: 6417
	Public krillVelocityY As MinMax

	' Token: 0x04001912 RID: 6418
	Public krillLaunchDelay As Single

	' Token: 0x04001913 RID: 6419
	Public krillGravity As Single

	' Token: 0x04001914 RID: 6420
	Public dragonTimeIn As Single

	' Token: 0x04001915 RID: 6421
	Public dragonTimeOut As Single

	' Token: 0x04001916 RID: 6422
	Public dragonLeaveDelay As Single

	' Token: 0x04001917 RID: 6423
	Public minerShootSpeed As Single

	' Token: 0x04001918 RID: 6424
	Public minerDescendTime As Single

	' Token: 0x04001919 RID: 6425
	Public minerRopeAscendTime As Single

	' Token: 0x0400191A RID: 6426
	Public minerShotDelay As MinMax

	' Token: 0x0400191B RID: 6427
	Public minerDistance As Single

	' Token: 0x0400191C RID: 6428
	Public wallFaceTravelTime As Single

	' Token: 0x0400191D RID: 6429
	Public wallAttackDelay As MinMax

	' Token: 0x0400191E RID: 6430
	Public wallProjectileXSpeed As Single

	' Token: 0x0400191F RID: 6431
	Public wallProjectileYSpeed As Single

	' Token: 0x04001920 RID: 6432
	Public wallProjectileGravity As Single

	' Token: 0x04001921 RID: 6433
	Public flamerCirSpeed As Single

	' Token: 0x04001922 RID: 6434
	Public flamerXSpeed As MinMax

	' Token: 0x04001923 RID: 6435
	Public flamerLoopSize As Single

	' Token: 0x04001924 RID: 6436
	Public fanVelocity As Single

	' Token: 0x04001925 RID: 6437
	Public fanWaitTime As MinMax

	' Token: 0x04001926 RID: 6438
	Public funWallTopDelayRange As MinMax

	' Token: 0x04001927 RID: 6439
	Public funWallBottomDelayRange As MinMax

	' Token: 0x04001928 RID: 6440
	Public funWallProjectileSpeed As Single

	' Token: 0x04001929 RID: 6441
	Public funWallMouthOpenTime As Single

	' Token: 0x0400192A RID: 6442
	Public funWallCarDelayRange As MinMax

	' Token: 0x0400192B RID: 6443
	Public funWallCarSpeed As Single

	' Token: 0x0400192C RID: 6444
	Public funWallTongueDelayRange As MinMax

	' Token: 0x0400192D RID: 6445
	Public funWallTongueLoopTime As Single

	' Token: 0x0400192E RID: 6446
	Public jackLaunchVelocity As Single

	' Token: 0x0400192F RID: 6447
	Public jackHomingMoveSpeed As Single

	' Token: 0x04001930 RID: 6448
	Public jackRotationSpeed As Single

	' Token: 0x04001931 RID: 6449
	Public jacktimeBeforeDeath As Single

	' Token: 0x04001932 RID: 6450
	Public jacktimeBeforeHoming As Single

	' Token: 0x04001933 RID: 6451
	Public jackEaseTime As Single

	' Token: 0x04001934 RID: 6452
	Public jackinDirectionString As String

	' Token: 0x04001935 RID: 6453
	Public jackinAppearDelay As MinMax

	' Token: 0x04001936 RID: 6454
	Public jackinDeathAppearDelay As MinMax

	' Token: 0x04001937 RID: 6455
	Public jackinWarningDuration As Single

	' Token: 0x04001938 RID: 6456
	Public jackinShootDelay As Single

	' Token: 0x04001939 RID: 6457
	Public tubaACount As Integer

	' Token: 0x0400193A RID: 6458
	Public tubaInitialDelay As Single

	' Token: 0x0400193B RID: 6459
	Public tubaMainDelayRange As MinMax

	' Token: 0x0400193C RID: 6460
	Public cannonSpeed As Single

	' Token: 0x0400193D RID: 6461
	Public cannonShotDelay As Single

	' Token: 0x0400193E RID: 6462
	Public bulletDeathTime As Single

	' Token: 0x0400193F RID: 6463
	Public pretzelXSpeedRange As MinMax

	' Token: 0x04001940 RID: 6464
	Public pretzelYSpeed As Single

	' Token: 0x04001941 RID: 6465
	Public pretzelGroundDelay As Single

	' Token: 0x04001942 RID: 6466
	Public arcadeAttackDelayInit As MinMax

	' Token: 0x04001943 RID: 6467
	Public arcadeAttackDelay As MinMax

	' Token: 0x04001944 RID: 6468
	Public arcadeBulletSpeed As Single

	' Token: 0x04001945 RID: 6469
	Public arcadeBulletReturnDelay As MinMax

	' Token: 0x04001946 RID: 6470
	Public arcadeBulletCount As Integer

	' Token: 0x04001947 RID: 6471
	Public arcadeBulletIndividualDelay As Single

	' Token: 0x04001948 RID: 6472
	Public magicianAppearDelayRange As MinMax

	' Token: 0x04001949 RID: 6473
	Public magicianDeathDelayRange As MinMax

	' Token: 0x0400194A RID: 6474
	Public magicianDurationAppear As Single

	' Token: 0x0400194B RID: 6475
	Public poleSpeedMovement As Single

	' Token: 0x02000432 RID: 1074
	Public Enum AimMode
		' Token: 0x0400194D RID: 6477
		AimedAtPlayer
		' Token: 0x0400194E RID: 6478
		ArcAimedAtPlayer
		' Token: 0x0400194F RID: 6479
		Straight
		' Token: 0x04001950 RID: 6480
		Spread
		' Token: 0x04001951 RID: 6481
		Arc
	End Enum

	' Token: 0x02000433 RID: 1075
	Public Enum LoopMode
		' Token: 0x04001953 RID: 6483
		PingPong
		' Token: 0x04001954 RID: 6484
		Repeat
		' Token: 0x04001955 RID: 6485
		Once
		' Token: 0x04001956 RID: 6486
		DelayAtPoint
	End Enum
End Class
