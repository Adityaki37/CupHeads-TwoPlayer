Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000AEC RID: 2796
Public Class DamageDealer
	' Token: 0x060043BA RID: 17338 RVA: 0x00240253 File Offset: 0x0023E653
	Public Sub New(damage As Single, damageRate As Single)
		Me.Setup(damage, damageRate)
	End Sub

	' Token: 0x060043BB RID: 17339 RVA: 0x00240290 File Offset: 0x0023E690
	Public Sub New(damage As Single, damageRate As Single, damagesPlayer As Boolean, damagesEnemy As Boolean, damagesOther As Boolean)
		Me.Setup(damage, damageRate, DamageDealer.DamageSource.Neutral, damagesPlayer, damagesEnemy, damagesOther, 1F)
	End Sub

	' Token: 0x060043BC RID: 17340 RVA: 0x002402E4 File Offset: 0x0023E6E4
	Public Sub New(damage As Single, damageRate As Single, damageSource As DamageDealer.DamageSource, damagesPlayer As Boolean, damagesEnemy As Boolean, damagesOther As Boolean)
		Me.Setup(damage, damageRate, damageSource, damagesPlayer, damagesEnemy, damagesOther, 1F)
	End Sub

	' Token: 0x060043BD RID: 17341 RVA: 0x00240338 File Offset: 0x0023E738
	Public Sub New(projectile As AbstractProjectile)
		Me.Setup(projectile.Damage, projectile.DamageRate, projectile.DamageSource, projectile.GetDamagesType(DamageReceiver.Type.Player), projectile.GetDamagesType(DamageReceiver.Type.Enemy), projectile.GetDamagesType(DamageReceiver.Type.Other), projectile.DamageMultiplier)
		Me.SetDirection(DamageDealer.Direction.Neutral, projectile.transform)
	End Sub

	' Token: 0x17000609 RID: 1545
	' (get) Token: 0x060043BE RID: 17342 RVA: 0x002403B7 File Offset: 0x0023E7B7
	' (set) Token: 0x060043BF RID: 17343 RVA: 0x002403BF File Offset: 0x0023E7BF
	Public Property DamageDealt As Single

	' Token: 0x1700060A RID: 1546
	' (get) Token: 0x060043C0 RID: 17344 RVA: 0x002403C8 File Offset: 0x0023E7C8
	' (set) Token: 0x060043C1 RID: 17345 RVA: 0x002403D0 File Offset: 0x0023E7D0
	Public Property DamageMultiplier As Single
		Get
			Return Me.damageMultiplier
		End Get
		Set(value As Single)
			Me.damageMultiplier = value
		End Set
	End Property

	' Token: 0x1700060B RID: 1547
	' (get) Token: 0x060043C2 RID: 17346 RVA: 0x002403D9 File Offset: 0x0023E7D9
	' (set) Token: 0x060043C3 RID: 17347 RVA: 0x002403E1 File Offset: 0x0023E7E1
	Public Property PlayerId As PlayerId
		Get
			Return Me.playerId
		End Get
		Set(value As PlayerId)
			Me.playerId = value
		End Set
	End Property

	' Token: 0x1700060C RID: 1548
	' (get) Token: 0x060043C4 RID: 17348 RVA: 0x002403EA File Offset: 0x0023E7EA
	' (set) Token: 0x060043C5 RID: 17349 RVA: 0x002403F2 File Offset: 0x0023E7F2
	Public Property isDLCWeapon As Boolean

	' Token: 0x140000BE RID: 190
	' (add) Token: 0x060043C6 RID: 17350 RVA: 0x002403FC File Offset: 0x0023E7FC
	' (remove) Token: 0x060043C7 RID: 17351 RVA: 0x00240434 File Offset: 0x0023E834
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDealDamage As DamageDealer.OnDealDamageHandler

	' Token: 0x060043C8 RID: 17352 RVA: 0x0024046A File Offset: 0x0023E86A
	Public Shared Function NewEnemy() As DamageDealer
		Return DamageDealer.NewEnemy(0.2F)
	End Function

	' Token: 0x060043C9 RID: 17353 RVA: 0x00240476 File Offset: 0x0023E876
	Public Shared Function NewEnemy(rate As Single) As DamageDealer
		Return New DamageDealer(1F, rate, DamageDealer.DamageSource.Enemy, True, False, False)
	End Function

	' Token: 0x060043CA RID: 17354 RVA: 0x00240487 File Offset: 0x0023E887
	Private Sub Setup(damage As Single, damageRate As Single)
		Me.Setup(damage, damageRate, DamageDealer.DamageSource.Neutral, True, False, False, 1F)
	End Sub

	' Token: 0x060043CB RID: 17355 RVA: 0x0024049A File Offset: 0x0023E89A
	Private Sub Setup(damage As Single, damageRate As Single, damageSource As DamageDealer.DamageSource)
		Me.Setup(damage, damageRate, damageSource, True, False, False, 1F)
	End Sub

	' Token: 0x060043CC RID: 17356 RVA: 0x002404B0 File Offset: 0x0023E8B0
	Private Sub Setup(damage As Single, damageRate As Single, damageSource As DamageDealer.DamageSource, damagesPlayer As Boolean, damagesEnemy As Boolean, damagesOther As Boolean, Optional damageMultiplier As Single = 1F)
		Me.damage = damage
		Me.damageRate = damageRate
		Me.damageMultiplier = damageMultiplier
		Me.damageTypes = New DamageDealer.DamageTypesManager()
		Me.SetDamageFlags(damagesPlayer, damagesEnemy, damagesOther)
		Me.SetDamageSource(damageSource)
		Me.timers = New Dictionary(Of Integer, Single)()
		Me.timersList = New List(Of Integer)()
		Me.StoneTime = -1F
	End Sub

	' Token: 0x060043CD RID: 17357 RVA: 0x00240512 File Offset: 0x0023E912
	Public Sub SetDamage(damage As Single)
		Me.damage = damage
	End Sub

	' Token: 0x060043CE RID: 17358 RVA: 0x0024051B File Offset: 0x0023E91B
	Public Sub SetRate(rate As Single)
		Me.damageRate = rate
	End Sub

	' Token: 0x060043CF RID: 17359 RVA: 0x00240524 File Offset: 0x0023E924
	Public Sub SetDamageSource(source As DamageDealer.DamageSource)
		Me.damageSource = source
	End Sub

	' Token: 0x060043D0 RID: 17360 RVA: 0x0024052D File Offset: 0x0023E92D
	Public Sub SetDamageFlags(damagesPlayer As Boolean, damagesEnemy As Boolean, damagesOther As Boolean)
		Me.damageTypes.Player = damagesPlayer
		Me.damageTypes.Enemies = damagesEnemy
		Me.damageTypes.Other = damagesOther
	End Sub

	' Token: 0x060043D1 RID: 17361 RVA: 0x00240553 File Offset: 0x0023E953
	Public Sub SetDirection(direction As DamageDealer.Direction, origin As Transform)
		Me.direction = direction
		Me.origin = origin
	End Sub

	' Token: 0x060043D2 RID: 17362 RVA: 0x00240564 File Offset: 0x0023E964
	Public Function DealDamage(hit As GameObject) As Single
		Dim damageReceiver As DamageReceiver = hit.GetComponent(Of DamageReceiver)()
		If damageReceiver Is Nothing Then
			Dim component As DamageReceiverChild = hit.GetComponent(Of DamageReceiverChild)()
			If component IsNot Nothing AndAlso component.enabled Then
				damageReceiver = component.Receiver
			End If
		End If
		If Not(damageReceiver IsNot Nothing) OrElse Not damageReceiver.enabled Then
			Return 0F
		End If
		Dim instanceID As Integer = damageReceiver.GetInstanceID()
		If Not Me.damageTypes.[GetType](damageReceiver.type) Then
			Return 0F
		End If
		If Not Me.timers.ContainsKey(instanceID) Then
			Me.timers.Add(instanceID, Me.damageRate)
			Me.timersList.Add(instanceID)
		ElseIf Me.damageRate = 0F Then
			Return 0F
		End If
		If Me.timers(instanceID) < Me.damageRate Then
			Return 0F
		End If
		Dim vector As Vector2 = If((Not(Me.origin IsNot Nothing)), Vector2.zero, Me.origin.position)
		Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(Me.damage * Me.damageMultiplier, Me.direction, vector, Me.damageSource)
		damageInfo.SetStoneTime(Me.StoneTime)
		damageReceiver.TakeDamage(damageInfo)
		Me.DamageDealt += Me.damage * Me.damageMultiplier
		Me.timers(damageReceiver.GetInstanceID()) = 0F
		If Me.OnDealDamage IsNot Nothing Then
			Me.OnDealDamage(Me.damage * Me.damageMultiplier, damageReceiver, Me)
		End If
		If Me.playerId <> PlayerId.None AndAlso damageReceiver.type = DamageReceiver.Type.Enemy Then
			DamageDealer.lastPlayer = Me.playerId
			DamageDealer.lastPlayerDamageSource = Me.damageSource
			If Me.damageSource <> DamageDealer.DamageSource.SmallPlane Then
				DamageDealer.didDamageWithNonSmallPlaneWeapon = True
			End If
			DamageDealer.lastDamageWasDLCWeapon = Me.isDLCWeapon
		End If
		Return Me.damage
	End Function

	' Token: 0x060043D3 RID: 17363 RVA: 0x0024075C File Offset: 0x0023EB5C
	Public Sub Update()
		For Each num As Integer In Me.timersList
			Dim dictionary As Dictionary(Of Integer, Single) = Me.timers
			Dim dictionary2 As Dictionary(Of Integer, Single) = dictionary
			Dim num2 As Integer = num
			Dim num3 As Integer = num2
			dictionary(num2) = dictionary2(num3) + CupheadTime.Delta
		Next
	End Sub

	' Token: 0x060043D4 RID: 17364 RVA: 0x002407D4 File Offset: 0x0023EBD4
	Public Sub FixedUpdate()
		For Each num As Integer In Me.timersList
			Dim dictionary As Dictionary(Of Integer, Single) = Me.timers
			Dim dictionary2 As Dictionary(Of Integer, Single) = dictionary
			Dim num2 As Integer = num
			Dim num3 As Integer = num2
			dictionary(num2) = dictionary2(num3) + CupheadTime.FixedDelta
		Next
	End Sub

	' Token: 0x1700060D RID: 1549
	' (get) Token: 0x060043D5 RID: 17365 RVA: 0x00240848 File Offset: 0x0023EC48
	' (set) Token: 0x060043D6 RID: 17366 RVA: 0x00240850 File Offset: 0x0023EC50
	Public Property StoneTime As Single

	' Token: 0x060043D7 RID: 17367 RVA: 0x00240859 File Offset: 0x0023EC59
	Public Sub SetStoneTime(stoneTime As Single)
		Me.StoneTime = stoneTime
	End Sub

	' Token: 0x0400496D RID: 18797
	Public Shared lastPlayerDamageSource As DamageDealer.DamageSource

	' Token: 0x0400496E RID: 18798
	Public Shared lastPlayer As PlayerId

	' Token: 0x0400496F RID: 18799
	Public Shared lastDamageWasDLCWeapon As Boolean

	' Token: 0x04004970 RID: 18800
	Public Shared didDamageWithNonSmallPlaneWeapon As Boolean

	' Token: 0x04004971 RID: 18801
	Private timers As Dictionary(Of Integer, Single)

	' Token: 0x04004972 RID: 18802
	Private timersList As List(Of Integer)

	' Token: 0x04004973 RID: 18803
	Private damage As Single = 1F

	' Token: 0x04004974 RID: 18804
	Private damageRate As Single = 1F

	' Token: 0x04004975 RID: 18805
	Private damageMultiplier As Single = 1F

	' Token: 0x04004976 RID: 18806
	Private direction As DamageDealer.Direction

	' Token: 0x04004977 RID: 18807
	Private origin As Transform

	' Token: 0x04004978 RID: 18808
	Private damageSource As DamageDealer.DamageSource

	' Token: 0x04004979 RID: 18809
	Private damageTypes As DamageDealer.DamageTypesManager

	' Token: 0x0400497A RID: 18810
	Private playerId As PlayerId = PlayerId.None

	' Token: 0x02000AED RID: 2797
	Public Enum Direction
		' Token: 0x0400497F RID: 18815
		Neutral
		' Token: 0x04004980 RID: 18816
		Left
		' Token: 0x04004981 RID: 18817
		Right
	End Enum

	' Token: 0x02000AEE RID: 2798
	Public Enum DamageSource
		' Token: 0x04004983 RID: 18819
		Neutral
		' Token: 0x04004984 RID: 18820
		Enemy
		' Token: 0x04004985 RID: 18821
		Ex
		' Token: 0x04004986 RID: 18822
		SmallPlane
		' Token: 0x04004987 RID: 18823
		Super
		' Token: 0x04004988 RID: 18824
		Pit
	End Enum

	' Token: 0x02000AEF RID: 2799
	' (Invoke) Token: 0x060043D9 RID: 17369
	Public Delegate Sub OnDealDamageHandler(damage As Single, receiver As DamageReceiver, dealer As DamageDealer)

	' Token: 0x02000AF0 RID: 2800
	Public Class DamageInfo
		' Token: 0x060043DC RID: 17372 RVA: 0x00240862 File Offset: 0x0023EC62
		Public Sub New(damage As Single, direction As DamageDealer.Direction, origin As Vector2, source As DamageDealer.DamageSource)
			Me.direction = direction
			Me.origin = origin
			Me.damageSource = source
			Me.damage = damage
			Me.stoneTime = -1F
		End Sub

		' Token: 0x1700060E RID: 1550
		' (get) Token: 0x060043DD RID: 17373 RVA: 0x00240892 File Offset: 0x0023EC92
		' (set) Token: 0x060043DE RID: 17374 RVA: 0x0024089A File Offset: 0x0023EC9A
		Public Property damage As Single

		' Token: 0x1700060F RID: 1551
		' (get) Token: 0x060043DF RID: 17375 RVA: 0x002408A3 File Offset: 0x0023ECA3
		' (set) Token: 0x060043E0 RID: 17376 RVA: 0x002408AB File Offset: 0x0023ECAB
		Public Property direction As DamageDealer.Direction

		' Token: 0x17000610 RID: 1552
		' (get) Token: 0x060043E1 RID: 17377 RVA: 0x002408B4 File Offset: 0x0023ECB4
		' (set) Token: 0x060043E2 RID: 17378 RVA: 0x002408BC File Offset: 0x0023ECBC
		Public Property origin As Vector2

		' Token: 0x17000611 RID: 1553
		' (get) Token: 0x060043E3 RID: 17379 RVA: 0x002408C5 File Offset: 0x0023ECC5
		' (set) Token: 0x060043E4 RID: 17380 RVA: 0x002408CD File Offset: 0x0023ECCD
		Public Property damageSource As DamageDealer.DamageSource

		' Token: 0x17000612 RID: 1554
		' (get) Token: 0x060043E5 RID: 17381 RVA: 0x002408D6 File Offset: 0x0023ECD6
		' (set) Token: 0x060043E6 RID: 17382 RVA: 0x002408DE File Offset: 0x0023ECDE
		Public Property stoneTime As Single

		' Token: 0x060043E7 RID: 17383 RVA: 0x002408E7 File Offset: 0x0023ECE7
		Public Sub SetStoneTime(stoneTime As Single)
			Me.stoneTime = stoneTime
		End Sub

		' Token: 0x060043E8 RID: 17384 RVA: 0x002408F0 File Offset: 0x0023ECF0
		Public Sub SetEditorPlayer()
			Me.damage *= 10F
		End Sub
	End Class

	' Token: 0x02000AF1 RID: 2801
	<Serializable()>
	Public Class DamageTypesManager
		' Token: 0x060043EA RID: 17386 RVA: 0x0024090C File Offset: 0x0023ED0C
		Public Function Copy() As DamageDealer.DamageTypesManager
			Return TryCast(MyBase.MemberwiseClone(), DamageDealer.DamageTypesManager)
		End Function

		' Token: 0x060043EB RID: 17387 RVA: 0x00240919 File Offset: 0x0023ED19
		Public Sub SetAll(b As Boolean)
			Me.Player = b
			Me.Enemies = b
			Me.Other = b
		End Sub

		' Token: 0x060043EC RID: 17388 RVA: 0x00240930 File Offset: 0x0023ED30
		Public Function OnlyPlayer() As DamageDealer.DamageTypesManager
			Me.SetAll(False)
			Me.Player = True
			Return Me
		End Function

		' Token: 0x060043ED RID: 17389 RVA: 0x00240941 File Offset: 0x0023ED41
		Public Function OnlyEnemies() As DamageDealer.DamageTypesManager
			Me.SetAll(False)
			Me.Enemies = True
			Return Me
		End Function

		' Token: 0x060043EE RID: 17390 RVA: 0x00240952 File Offset: 0x0023ED52
		Public Function PlayerProjectileDefault() As DamageDealer.DamageTypesManager
			Me.SetAll(False)
			Me.Enemies = True
			Me.Other = True
			Return Me
		End Function

		' Token: 0x060043EF RID: 17391 RVA: 0x0024096A File Offset: 0x0023ED6A
		Public Function [GetType](type As DamageReceiver.Type) As Boolean
			Select Case type
				Case DamageReceiver.Type.Enemy
					Return Me.Enemies
				Case DamageReceiver.Type.Player
					Return Me.Player
				Case DamageReceiver.Type.Other
					Return Me.Other
				Case Else
					Return False
			End Select
		End Function

		' Token: 0x060043F0 RID: 17392 RVA: 0x0024099C File Offset: 0x0023ED9C
		Public Overrides Function ToString() As String
			Return String.Concat(New Object() { "Player:", Me.Player, ", Enemies:", Me.Enemies, ", Other:", Me.Other })
		End Function

		' Token: 0x0400498E RID: 18830
		Public Player As Boolean

		' Token: 0x0400498F RID: 18831
		Public Enemies As Boolean

		' Token: 0x04004990 RID: 18832
		Public Other As Boolean
	End Class
End Class
