Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004CE RID: 1230
Public Class AirshipCrabLevelBubbles
	Inherits AbstractProjectile

	' Token: 0x060014ED RID: 5357 RVA: 0x000BBA94 File Offset: 0x000B9E94
	Public Sub Init(pos As Vector2, properties As LevelProperties.AirshipCrab.Bubbles, speed As Single)
		Me.properties = properties
		MyBase.transform.position = pos
		Me.speed = speed
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060014EE RID: 5358 RVA: 0x000BBAC2 File Offset: 0x000B9EC2
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060014EF RID: 5359 RVA: 0x000BBAE0 File Offset: 0x000B9EE0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060014F0 RID: 5360 RVA: 0x000BBAF8 File Offset: 0x000B9EF8
	Private Iterator Function move_cr() As IEnumerator
		Me.speedY = Me.properties.sinWaveStrength
		Dim t As Single = Global.UnityEngine.Random.Range(0F, 6.2831855F)
		While MyBase.transform.position.x > -640F
			Dim pos As Vector3 = MyBase.transform.position
			pos.x = Mathf.MoveTowards(MyBase.transform.position.x, -640F, Me.speed * CupheadTime.Delta)
			MyBase.transform.position = New Vector3(pos.x, MyBase.transform.position.y + Mathf.Sin(t) * Me.speedY * CupheadTime.Delta * 60F, 0F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060014F1 RID: 5361 RVA: 0x000BBB13 File Offset: 0x000B9F13
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001E4C RID: 7756
	Private properties As LevelProperties.AirshipCrab.Bubbles

	' Token: 0x04001E4D RID: 7757
	Private speed As Single

	' Token: 0x04001E4E RID: 7758
	Private speedY As Single
End Class
