Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000851 RID: 2129
Public Class VeggiesLevelOnionTearProjectile
	Inherits AbstractProjectile

	' Token: 0x0600315E RID: 12638 RVA: 0x001CE1F8 File Offset: 0x001CC5F8
	Public Function Create(time As Single, x As Single) As AbstractProjectile
		Dim veggiesLevelOnionTearProjectile As VeggiesLevelOnionTearProjectile = TryCast(MyBase.Create(New Vector2(x, 420F)), VeggiesLevelOnionTearProjectile)
		veggiesLevelOnionTearProjectile.direction = Me.direction
		veggiesLevelOnionTearProjectile.time = time
		veggiesLevelOnionTearProjectile.Init()
		Return veggiesLevelOnionTearProjectile
	End Function

	' Token: 0x0600315F RID: 12639 RVA: 0x001CE236 File Offset: 0x001CC636
	Protected Sub Init()
		MyBase.StartCoroutine(Me.projectile_cr())
	End Sub

	' Token: 0x06003160 RID: 12640 RVA: 0x001CE245 File Offset: 0x001CC645
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003161 RID: 12641 RVA: 0x001CE270 File Offset: 0x001CC670
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.transform.SetScale(New Single?(CSng(If((Not MathUtils.RandomBool()), (-1), 1))), Nothing, Nothing)
	End Sub

	' Token: 0x06003162 RID: 12642 RVA: 0x001CE2B8 File Offset: 0x001CC6B8
	Private Iterator Function projectile_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startY As Single = MyBase.transform.position.y
		Dim endY As Single = CSng(Level.Current.Ground)
		Dim calculatedTime As Single = Me.time
		Dim t As Single = 0F
		While t < calculatedTime
			Dim val As Single = t / calculatedTime
			Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInQuad, startY, endY, val)
			MyBase.transform.SetPosition(Nothing, New Single?(y), Nothing)
			t += CupheadTime.FixedDelta
			Yield wait
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(endY), Nothing)
		AudioManager.Play("level_veggies_onion_teardrop")
		Me.Die()
		Return
	End Function

	' Token: 0x040039EB RID: 14827
	Private Const startY As Single = 420F

	' Token: 0x040039EC RID: 14828
	Private time As Single

	' Token: 0x040039ED RID: 14829
	Private direction As Integer
End Class
