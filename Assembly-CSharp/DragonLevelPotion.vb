Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005FB RID: 1531
Public Class DragonLevelPotion
	Inherits AbstractProjectile

	' Token: 0x17000377 RID: 887
	' (get) Token: 0x06001E78 RID: 7800 RVA: 0x001191A0 File Offset: 0x001175A0
	' (set) Token: 0x06001E79 RID: 7801 RVA: 0x001191A8 File Offset: 0x001175A8
	Public Property state As DragonLevelPotion.State

	' Token: 0x06001E7A RID: 7802 RVA: 0x001191B4 File Offset: 0x001175B4
	Public Sub Init(pos As Vector2, hp As Single, rotation As Single, properties As LevelProperties.Dragon.Potions)
		MyBase.transform.position = pos
		Me.hp = hp
		Me.properties = properties
		MyBase.transform.SetScale(New Single?(properties.potionScale), New Single?(properties.potionScale), New Single?(properties.potionScale))
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(rotation))
		Me.state = DragonLevelPotion.State.Alive
		Me.moveRoutine = MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001E7B RID: 7803 RVA: 0x0011924E File Offset: 0x0011764E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001E7C RID: 7804 RVA: 0x00119279 File Offset: 0x00117679
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E7D RID: 7805 RVA: 0x00119297 File Offset: 0x00117697
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001E7E RID: 7806 RVA: 0x001192B8 File Offset: 0x001176B8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> DragonLevelPotion.State.Dead Then
			Me.state = DragonLevelPotion.State.Dead
			MyBase.StopCoroutine(Me.moveRoutine)
			MyBase.StartCoroutine(Me.handle_die_cr())
		End If
	End Sub

	' Token: 0x06001E7F RID: 7807 RVA: 0x00119314 File Offset: 0x00117714
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.position += MyBase.transform.right * Me.properties.potionSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001E80 RID: 7808 RVA: 0x00119330 File Offset: 0x00117730
	Private Iterator Function handle_die_cr() As IEnumerator
		Dim potionType As DragonLevelPotion.PotionType = Me.type
		If potionType <> DragonLevelPotion.PotionType.Horizontal Then
			If potionType <> DragonLevelPotion.PotionType.Vertical Then
				If potionType = DragonLevelPotion.PotionType.Both Then
					Me.SpawnProjectile(Vector3.right)
					Me.SpawnProjectile(-Vector3.right)
					Me.SpawnProjectile(Vector3.up)
					Me.SpawnProjectile(-Vector3.up)
				End If
			Else
				Me.SpawnProjectile(Vector3.up)
				Me.SpawnProjectile(-Vector3.up)
			End If
		Else
			Me.SpawnProjectile(Vector3.right)
			Me.SpawnProjectile(-Vector3.right)
		End If
		MyBase.animator.SetTrigger("Explode")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001E81 RID: 7809 RVA: 0x0011934C File Offset: 0x0011774C
	Private Sub SpawnProjectile(direction As Vector3)
		Dim num As Single = MathUtils.DirectionToAngle(direction)
		Me.bulletPrefab.Create(MyBase.transform.position, num, Me.properties.spitBulletSpeed).transform.SetScale(New Single?(Me.properties.explosionBulletScale), New Single?(Me.properties.explosionBulletScale), New Single?(Me.properties.explosionBulletScale))
	End Sub

	' Token: 0x06001E82 RID: 7810 RVA: 0x001193C6 File Offset: 0x001177C6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bulletPrefab = Nothing
	End Sub

	' Token: 0x04002751 RID: 10065
	Private Const ExplodeTrigger As String = "Explode"

	' Token: 0x04002752 RID: 10066
	<SerializeField()>
	Private bulletPrefab As BasicProjectile

	' Token: 0x04002753 RID: 10067
	Public type As DragonLevelPotion.PotionType

	' Token: 0x04002755 RID: 10069
	Private properties As LevelProperties.Dragon.Potions

	' Token: 0x04002756 RID: 10070
	Private damageReceiver As DamageReceiver

	' Token: 0x04002757 RID: 10071
	Private hp As Single

	' Token: 0x04002758 RID: 10072
	Private moveRoutine As Coroutine

	' Token: 0x020005FC RID: 1532
	Public Enum PotionType
		' Token: 0x0400275A RID: 10074
		Horizontal
		' Token: 0x0400275B RID: 10075
		Vertical
		' Token: 0x0400275C RID: 10076
		Both
	End Enum

	' Token: 0x020005FD RID: 1533
	Public Enum State
		' Token: 0x0400275E RID: 10078
		Alive
		' Token: 0x0400275F RID: 10079
		Dead
	End Enum
End Class
