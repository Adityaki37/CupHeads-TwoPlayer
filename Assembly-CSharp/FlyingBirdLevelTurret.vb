Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200062B RID: 1579
Public Class FlyingBirdLevelTurret
	Inherits AbstractCollidableObject

	' Token: 0x0600202A RID: 8234 RVA: 0x00127B44 File Offset: 0x00125F44
	Public Function Create(pos As Vector2, properties As FlyingBirdLevelTurret.Properties) As FlyingBirdLevelTurret
		Dim flyingBirdLevelTurret As FlyingBirdLevelTurret = Me.InstantiatePrefab(Of FlyingBirdLevelTurret)()
		flyingBirdLevelTurret.transform.position = pos
		flyingBirdLevelTurret.properties = properties
		flyingBirdLevelTurret.Init()
		Return flyingBirdLevelTurret
	End Function

	' Token: 0x17000380 RID: 896
	' (get) Token: 0x0600202B RID: 8235 RVA: 0x00127B77 File Offset: 0x00125F77
	' (set) Token: 0x0600202C RID: 8236 RVA: 0x00127B7F File Offset: 0x00125F7F
	Public Property state As FlyingBirdLevelTurret.State

	' Token: 0x0600202D RID: 8237 RVA: 0x00127B88 File Offset: 0x00125F88
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
	End Sub

	' Token: 0x0600202E RID: 8238 RVA: 0x00127BC1 File Offset: 0x00125FC1
	Private Sub Init()
		Me.startPos = MyBase.transform.position
		MyBase.StartCoroutine(Me.go_cr())
		MyBase.StartCoroutine(Me.y_cr())
	End Sub

	' Token: 0x0600202F RID: 8239 RVA: 0x00127BF4 File Offset: 0x00125FF4
	Private Sub Shoot()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		If [next] Is Nothing OrElse [next].transform Is Nothing Then
			Return
		End If
		Me.aim.LookAt2D([next].transform)
		Dim basicProjectile As BasicProjectile = Me.childPrefab.Create(MyBase.transform.position, Me.aim.transform.eulerAngles.z, Me.properties.bulletSpeed)
		basicProjectile.CollisionDeath.OnlyPlayer()
		basicProjectile.DamagesType.OnlyPlayer()
	End Sub

	' Token: 0x06002030 RID: 8240 RVA: 0x00127C8C File Offset: 0x0012608C
	Private Iterator Function y_cr() As IEnumerator
		Dim start As Single = Me.startPos.y + Me.properties.floatRange / 2F
		Dim [end] As Single = Me.startPos.y - Me.properties.floatRange / 2F
		MyBase.transform.SetPosition(Nothing, New Single?(start), Nothing)
		Dim t As Single = 0F
		While True
			t = 0F
			While t < Me.properties.floatTime
				Dim val As Single = t / Me.properties.floatTime
				MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
			While t < Me.properties.floatTime
				Dim val2 As Single = t / Me.properties.floatTime
				MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, [end], start, val2)), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002031 RID: 8241 RVA: 0x00127CA8 File Offset: 0x001260A8
	Private Iterator Function go_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.properties.inTime
			Dim val As Single = t / Me.properties.inTime
			MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, Me.startPos.x, Me.properties.x, val)), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetPosition(New Single?(Me.properties.x), Nothing, Nothing)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.bulletDelay)
			Me.Shoot()
		End While
		Return
	End Function

	' Token: 0x040028A4 RID: 10404
	<SerializeField()>
	Private childPrefab As BasicProjectile

	' Token: 0x040028A5 RID: 10405
	Private startPos As Vector2

	' Token: 0x040028A6 RID: 10406
	Private properties As FlyingBirdLevelTurret.Properties

	' Token: 0x040028A7 RID: 10407
	Private aim As Transform

	' Token: 0x0200062C RID: 1580
	Public Enum State
		' Token: 0x040028A9 RID: 10409
		Alive
		' Token: 0x040028AA RID: 10410
		Dying
		' Token: 0x040028AB RID: 10411
		Dead
		' Token: 0x040028AC RID: 10412
		Respawn
	End Enum

	' Token: 0x0200062D RID: 1581
	Public Class Properties
		' Token: 0x06002032 RID: 8242 RVA: 0x00127CC3 File Offset: 0x001260C3
		Public Sub New(health As Single, inTime As Single, x As Single, bulletSpeed As Single, bulletDelay As Single, floatRange As Single, floatTime As Single)
			Me.health = health
			Me.inTime = inTime
			Me.x = x
			Me.bulletSpeed = bulletSpeed
			Me.bulletDelay = bulletDelay
			Me.floatRange = floatRange
			Me.floatTime = floatTime
		End Sub

		' Token: 0x040028AD RID: 10413
		Public health As Single

		' Token: 0x040028AE RID: 10414
		Public inTime As Single

		' Token: 0x040028AF RID: 10415
		Public x As Single

		' Token: 0x040028B0 RID: 10416
		Public bulletSpeed As Single

		' Token: 0x040028B1 RID: 10417
		Public bulletDelay As Single

		' Token: 0x040028B2 RID: 10418
		Public floatRange As Single

		' Token: 0x040028B3 RID: 10419
		Public floatTime As Single
	End Class
End Class
