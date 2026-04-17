Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000886 RID: 2182
Public Class TreePlatformingLevelBeetle
	Inherits PlatformingLevelPathMovementEnemy

	' Token: 0x1700043D RID: 1085
	' (get) Token: 0x060032A9 RID: 12969 RVA: 0x001D70A5 File Offset: 0x001D54A5
	' (set) Token: 0x060032AA RID: 12970 RVA: 0x001D70AD File Offset: 0x001D54AD
	Public Property isActivated As Boolean

	' Token: 0x1700043E RID: 1086
	' (get) Token: 0x060032AB RID: 12971 RVA: 0x001D70B6 File Offset: 0x001D54B6
	' (set) Token: 0x060032AC RID: 12972 RVA: 0x001D70BE File Offset: 0x001D54BE
	Public Property onCamera As Boolean

	' Token: 0x060032AD RID: 12973 RVA: 0x001D70C7 File Offset: 0x001D54C7
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isActivated = False
	End Sub

	' Token: 0x060032AE RID: 12974 RVA: 0x001D70D6 File Offset: 0x001D54D6
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.check_hit_box_cr())
		If Me.sprite IsNot Nothing Then
			MyBase.StartCoroutine(Me.motion_cr())
		End If
	End Sub

	' Token: 0x060032AF RID: 12975 RVA: 0x001D710C File Offset: 0x001D550C
	Protected Overrides Sub Die()
		If Me.explosion IsNot Nothing Then
			Me.explosion.Create(MyBase.transform.position)
		End If
		Me.hasStarted = False
		Me.onCamera = False
		AudioManager.[Stop]("level_platform_beetle_idle_loop")
		AudioManager.Play("level_platform_beetle_death")
		Me.emitAudioFromObject.Add("level_platform_beetle_death")
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x060032B0 RID: 12976 RVA: 0x001D717F File Offset: 0x001D557F
	Public Sub Activate()
		Me.isActivated = True
		Me.PrepareBeetle()
	End Sub

	' Token: 0x060032B1 RID: 12977 RVA: 0x001D718E File Offset: 0x001D558E
	Public Sub Deactivate()
		Me.isActivated = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.ResetStartingCondition()
	End Sub

	' Token: 0x060032B2 RID: 12978 RVA: 0x001D71AC File Offset: 0x001D55AC
	Private Sub PrepareBeetle()
		Me.pathIndex = 1
		Me.startPosition = MyBase.allValues(0)
		MyBase.StartFromCustom()
		MyBase.StartCoroutine(Me.check_for_death_cr())
		If(Me.pathIndex - 1) Mod 2 <> 0 Then
			MyBase.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		Else
			MyBase.transform.SetScale(New Single?(1F), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x060032B3 RID: 12979 RVA: 0x001D7243 File Offset: 0x001D5643
	Protected Overrides Sub OnStart()
		MyBase.OnStart()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
	End Sub

	' Token: 0x060032B4 RID: 12980 RVA: 0x001D7257 File Offset: 0x001D5657
	Protected Overrides Sub EndPath()
		MyBase.EndPath()
		Me.Deactivate()
	End Sub

	' Token: 0x060032B5 RID: 12981 RVA: 0x001D7268 File Offset: 0x001D5668
	Private Sub Flip()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(MyBase.transform.localScale.y), New Single?(MyBase.transform.localScale.z))
	End Sub

	' Token: 0x060032B6 RID: 12982 RVA: 0x001D72C9 File Offset: 0x001D56C9
	Public Sub PlayIdleSFX()
		If Not AudioManager.CheckIfPlaying("level_platform_beetle_idle_loop") Then
			AudioManager.PlayLoop("level_platform_beetle_idle_loop")
		End If
		Me.emitAudioFromObject.Add("level_platform_beetle_idle_loop")
	End Sub

	' Token: 0x060032B7 RID: 12983 RVA: 0x001D72F4 File Offset: 0x001D56F4
	Private Iterator Function check_for_death_cr() As IEnumerator
		While MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 50F
			Yield Nothing
		End While
		Me.hasStarted = False
		Me.onCamera = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060032B8 RID: 12984 RVA: 0x001D7310 File Offset: 0x001D5710
	Private Iterator Function check_hit_box_cr() As IEnumerator
		While True
			If MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 50F AndAlso Me.hasStarted Then
				Me.sprite.GetComponent(Of SpriteRenderer)().enabled = True
				If MyBase.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 50F Then
					If Me.hasStarted Then
						Me.onCamera = True
					End If
				Else
					Me.onCamera = False
				End If
			Else
				Me.sprite.GetComponent(Of SpriteRenderer)().enabled = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032B9 RID: 12985 RVA: 0x001D732C File Offset: 0x001D572C
	Private Iterator Function motion_cr() As IEnumerator
		Dim time As Single = 0.1F
		Dim t As Single = 0F
		Dim amount As Single = 2F
		While True
			While t < time
				While Not Me.isActivated OrElse PauseManager.state = PauseManager.State.Paused
					Yield Nothing
				End While
				t += CupheadTime.Delta
				Me.sprite.transform.AddPosition(0F, amount, 0F)
				Yield Nothing
			End While
			t = 0F
			amount = -amount
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003AE8 RID: 15080
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04003AE9 RID: 15081
	<SerializeField()>
	Private sprite As SpriteRenderer
End Class
