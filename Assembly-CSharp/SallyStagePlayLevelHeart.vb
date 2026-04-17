Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007AE RID: 1966
Public Class SallyStagePlayLevelHeart
	Inherits AbstractProjectile

	' Token: 0x06002C2E RID: 11310 RVA: 0x0019FA24 File Offset: 0x0019DE24
	Protected Overrides Sub Update()
		Me.damageDealer.Update()
		MyBase.Update()
	End Sub

	' Token: 0x06002C2F RID: 11311 RVA: 0x0019FA38 File Offset: 0x0019DE38
	Public Sub InitHeart(properties As LevelProperties.SallyStagePlay, direction As Integer, isParryable As Boolean)
		Me.properties = properties
		Me.time = 0F
		Me.direction = direction
		Me.pos = MyBase.transform.position
		If Not isParryable Then
			MyBase.GetComponent(Of SpriteRenderer)().color = Color.blue
		Else
			Me.SetParryable(True)
		End If
		MyBase.StartCoroutine(Me.wave_cr())
	End Sub

	' Token: 0x06002C30 RID: 11312 RVA: 0x0019FAA0 File Offset: 0x0019DEA0
	Private Iterator Function wave_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		While True
			Dim newPos As Vector3 = Me.pos
			newPos.y = Mathf.Sin(Me.time * Me.properties.CurrentState.kiss.sineWaveSpeed) * Me.properties.CurrentState.kiss.sineWaveStrength
			MyBase.transform.position = newPos
			Me.pos += Vector3.left * CSng(Me.direction) * Me.properties.CurrentState.kiss.heartSpeed * CupheadTime.Delta
			Me.time += CupheadTime.Delta
			Yield Nothing
			If MyBase.transform.position.x < CSng((Level.Current.Left - 150)) OrElse MyBase.transform.position.x > CSng((Level.Current.Right + 150)) Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
		End While
		Return
	End Function

	' Token: 0x06002C31 RID: 11313 RVA: 0x0019FABB File Offset: 0x0019DEBB
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x06002C32 RID: 11314 RVA: 0x0019FAE4 File Offset: 0x0019DEE4
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x040034DB RID: 13531
	Private direction As Integer

	' Token: 0x040034DC RID: 13532
	Private time As Single

	' Token: 0x040034DD RID: 13533
	Private pos As Vector3

	' Token: 0x040034DE RID: 13534
	Private properties As LevelProperties.SallyStagePlay
End Class
