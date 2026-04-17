Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000746 RID: 1862
Public Class RetroArcadeMissileMan
	Inherits RetroArcadeEnemy

	' Token: 0x06002891 RID: 10385 RVA: 0x0017A672 File Offset: 0x00178A72
	Public Sub LevelInit(properties As LevelProperties.RetroArcade)
		Me.properties = properties
		Me.hp = properties.CurrentState.missile.hp
	End Sub

	' Token: 0x06002892 RID: 10386 RVA: 0x0017A694 File Offset: 0x00178A94
	Public Sub StartMissile()
		MyBase.gameObject.SetActive(True)
		Me.missile = Global.UnityEngine.[Object].Instantiate(Of RetroArcadeMissile)(Me.missilePrefab)
		Me.missile.Init(Me.shootRootLeft.position, -90F, Me.properties.CurrentState.missile, Me.pivotPoint.position)
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.fire_missiles_cr())
	End Sub

	' Token: 0x06002893 RID: 10387 RVA: 0x0017A714 File Offset: 0x00178B14
	Private Iterator Function move_cr() As IEnumerator
		Dim time As Single = Me.properties.CurrentState.missile.manMoveTime
		Dim movingRight As Boolean = Rand.Bool()
		While True
			Dim t As Single = 0F
			Dim start As Single = MyBase.transform.position.x
			Dim [end] As Single
			If movingRight Then
				[end] = 80F
			Else
				[end] = -80F
			End If
			While t < time
				Dim val As Single = t / time
				MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, [end], val)), Nothing, Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			MyBase.transform.SetPosition(New Single?([end]), Nothing, Nothing)
			movingRight = Not movingRight
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002894 RID: 10388 RVA: 0x0017A730 File Offset: 0x00178B30
	Private Iterator Function fire_missiles_cr() As IEnumerator
		Dim dirString As String() = Me.properties.CurrentState.missile.directionString.Split(New Char() { ","c })
		Dim dirIndex As Integer = Global.UnityEngine.Random.Range(0, dirString.Length)
		Dim onRight As Boolean = False
		While True
			onRight = dirString(dirIndex) = "R"
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.missile.timerRelease.RandomFloat())
			Yield Nothing
			Me.missile.StartCircle(onRight, Me.pivotPoint.position)
			dirIndex = (dirIndex + 1) Mod dirString.Length
		End While
		Return
	End Function

	' Token: 0x06002895 RID: 10389 RVA: 0x0017A74B File Offset: 0x00178B4B
	Public Overrides Sub Dead()
		MyBase.Dead()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(Me.missile.gameObject)
		Me.properties.DealDamageToNextNamedState()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04003165 RID: 12645
	<SerializeField()>
	Private missilePrefab As RetroArcadeMissile

	' Token: 0x04003166 RID: 12646
	<SerializeField()>
	Private shootRootLeft As Transform

	' Token: 0x04003167 RID: 12647
	<SerializeField()>
	Private shootRootRight As Transform

	' Token: 0x04003168 RID: 12648
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x04003169 RID: 12649
	Private Const MAX_X_POS As Single = 80F

	' Token: 0x0400316A RID: 12650
	Private properties As LevelProperties.RetroArcade

	' Token: 0x0400316B RID: 12651
	Private missile As RetroArcadeMissile
End Class
