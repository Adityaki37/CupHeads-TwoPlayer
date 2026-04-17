Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000642 RID: 1602
Public Class FlyingBlimpLevelStars
	Inherits AbstractProjectile

	' Token: 0x17000388 RID: 904
	' (get) Token: 0x060020E3 RID: 8419 RVA: 0x0012FF6F File Offset: 0x0012E36F
	' (set) Token: 0x060020E4 RID: 8420 RVA: 0x0012FF77 File Offset: 0x0012E377
	Public Property state As FlyingBlimpLevelStars.State

	' Token: 0x060020E5 RID: 8421 RVA: 0x0012FF80 File Offset: 0x0012E380
	Public Function Create(pos As Vector2, properties As LevelProperties.FlyingBlimp.Stars) As FlyingBlimpLevelStars
		Dim flyingBlimpLevelStars As FlyingBlimpLevelStars = TryCast(MyBase.Create(), FlyingBlimpLevelStars)
		flyingBlimpLevelStars.properties = properties
		flyingBlimpLevelStars.transform.position = pos
		Return flyingBlimpLevelStars
	End Function

	' Token: 0x060020E6 RID: 8422 RVA: 0x0012FFB2 File Offset: 0x0012E3B2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060020E7 RID: 8423 RVA: 0x0012FFD0 File Offset: 0x0012E3D0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060020E8 RID: 8424 RVA: 0x0012FFF0 File Offset: 0x0012E3F0
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
		Dim num As Single = CSng(Global.UnityEngine.Random.Range(0, 2))
		Me.starFx = Global.UnityEngine.[Object].Instantiate(Of Transform)(Me.starFXPrefab)
		Me.starFx.transform.parent = MyBase.transform
		Dim position As Vector3 = MyBase.transform.position
		If num = 0F Then
			MyBase.transform.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
			Me.starFx.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
			position.x = MyBase.transform.position.x + 70F
		Else
			position.x = MyBase.transform.position.x - 10F
			Me.starFx.SetScale(New Single?(1F), New Single?(-1F), New Single?(1F))
		End If
		Me.starFx.transform.position = position
	End Sub

	' Token: 0x060020E9 RID: 8425 RVA: 0x00130128 File Offset: 0x0012E528
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim speed As Single = Me.properties.speedX.RandomFloat()
		Dim angle As Single = 0F
		While MyBase.transform.position.x > -840F
			angle += Me.properties.speedY * CupheadTime.FixedDelta
			If CupheadTime.Delta IsNot 0F Then
				Dim moveY As Vector3 = New Vector3(0F, Mathf.Sin(angle) * Me.properties.sineSize)
				Dim moveX As Vector3 = MyBase.transform.right * -speed * CupheadTime.FixedDelta
				MyBase.transform.position += moveX + moveY
			End If
			Yield wait
		End While
		Me.Die()
		Yield wait
		Return
	End Function

	' Token: 0x060020EA RID: 8426 RVA: 0x00130143 File Offset: 0x0012E543
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x0400297C RID: 10620
	<SerializeField()>
	Private starFXPrefab As Transform

	' Token: 0x0400297D RID: 10621
	Private starFx As Transform

	' Token: 0x0400297E RID: 10622
	Private spawnPoint As Vector3

	' Token: 0x0400297F RID: 10623
	Private properties As LevelProperties.FlyingBlimp.Stars

	' Token: 0x02000643 RID: 1603
	Public Enum State
		' Token: 0x04002981 RID: 10625
		Unspawned
		' Token: 0x04002982 RID: 10626
		Spawned
	End Enum
End Class
