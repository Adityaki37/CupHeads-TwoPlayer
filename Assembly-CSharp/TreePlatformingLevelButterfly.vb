Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000888 RID: 2184
Public Class TreePlatformingLevelButterfly
	Inherits AbstractPausableComponent

	' Token: 0x1700043F RID: 1087
	' (get) Token: 0x060032C1 RID: 12993 RVA: 0x001D78A4 File Offset: 0x001D5CA4
	' (set) Token: 0x060032C2 RID: 12994 RVA: 0x001D78AC File Offset: 0x001D5CAC
	Public Property isActive As Boolean

	' Token: 0x060032C3 RID: 12995 RVA: 0x001D78B8 File Offset: 0x001D5CB8
	Private Sub Start()
		Me.maxCounter = Global.UnityEngine.Random.Range(4, 7)
		If Me.sprite3.GetComponent(Of ParrySwitch)() IsNot Nothing Then
			AddHandler Me.sprite3.GetComponent(Of ParrySwitch)().OnActivate, AddressOf Me.Deactivate
		End If
	End Sub

	' Token: 0x060032C4 RID: 12996 RVA: 0x001D7904 File Offset: 0x001D5D04
	Public Sub Init(velocity As Vector2, scale As Single, color As Integer, velMinMax As MinMax)
		Me.isActive = True
		MyBase.transform.SetScale(New Single?(scale), Nothing, Nothing)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(If((scale >= 0F), MyBase.transform.eulerAngles.z, (-MyBase.transform.eulerAngles.z))))
		Me.velocity = velocity
		Me.velMinMax = velMinMax
		Me.SelectColor(color)
		Me.Setup()
	End Sub

	' Token: 0x060032C5 RID: 12997 RVA: 0x001D79B0 File Offset: 0x001D5DB0
	Private Sub Setup()
		Dim text As String = "P" + Global.UnityEngine.Random.Range(1, 5).ToStringInvariant()
		MyBase.animator.Play(text)
		MyBase.StartCoroutine(Me.check_dist_cr())
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.switch_y_cr())
		MyBase.StartCoroutine(Me.adjust_x_speed(Me.velMinMax))
	End Sub

	' Token: 0x060032C6 RID: 12998 RVA: 0x001D7A1A File Offset: 0x001D5E1A
	Public Sub Deactivate()
		Me.isActive = False
		Me.StopAllCoroutines()
		Me.sprite1.SetActive(False)
		Me.sprite2.SetActive(False)
		Me.sprite3.SetActive(False)
	End Sub

	' Token: 0x060032C7 RID: 12999 RVA: 0x001D7A50 File Offset: 0x001D5E50
	Private Sub SelectColor(color As Integer)
		Me.sprite1.SetActive(False)
		Me.sprite2.SetActive(False)
		Me.sprite3.SetActive(False)
		If color <> 1 Then
			If color <> 2 Then
				If color = 3 Then
					Me.sprite3.SetActive(True)
				End If
			Else
				Me.sprite2.SetActive(True)
			End If
		Else
			Me.sprite1.SetActive(True)
		End If
	End Sub

	' Token: 0x060032C8 RID: 13000 RVA: 0x001D7AD0 File Offset: 0x001D5ED0
	Private Iterator Function move_cr() As IEnumerator
		While True
			Me.frameTime += CupheadTime.Delta
			If Me.frameTime > 0.083333336F Then
				Me.frameTime -= 0.083333336F
				Dim vector As Vector2 = MyBase.transform.position
				vector += Me.velocity * CupheadTime.Delta
				MyBase.transform.position = vector
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032C9 RID: 13001 RVA: 0x001D7AEC File Offset: 0x001D5EEC
	Private Iterator Function switch_y_cr() As IEnumerator
		Dim time As Single = Global.UnityEngine.Random.Range(1F, 2F)
		Dim t As Single = 0F
		Dim startVel As Single = Me.velocity.y
		Dim endVel As Single = -Me.velocity.y
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(3F, 6F))
			While t < time
				t += CupheadTime.Delta
				Me.velocity.y = Mathf.Lerp(startVel, endVel, t / time)
				Yield Nothing
			End While
			Me.velocity.y = endVel
			t = 0F
			startVel = Me.velocity.y
			endVel = -Me.velocity.y
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032CA RID: 13002 RVA: 0x001D7B08 File Offset: 0x001D5F08
	Private Iterator Function adjust_x_speed(adjustment As MinMax) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = Global.UnityEngine.Random.Range(1F, 2F)
		Dim startVel As Single = Me.velocity.x
		Dim endVel As Single = If((Mathf.Sign(Me.velocity.x) <> 1F), (-adjustment.RandomFloat()), adjustment.RandomFloat())
		While True
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(4F, 6F))
			While t < time
				Me.velocity.x = Mathf.Lerp(startVel, endVel, time)
				Yield Nothing
			End While
			Me.velocity.x = endVel
			endVel = If((Mathf.Sign(Me.velocity.x) <> 1F), (-adjustment.RandomFloat()), adjustment.RandomFloat())
			startVel = Me.velocity.x
			t = 0F
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032CB RID: 13003 RVA: 0x001D7B2C File Offset: 0x001D5F2C
	Private Iterator Function check_dist_cr() As IEnumerator
		While True
			Dim dist As Single = Vector3.Distance(CupheadLevelCamera.Current.transform.position, MyBase.transform.position)
			If dist > 2000F Then
				Exit For
			End If
			Yield Nothing
		End While
		Me.Deactivate()
		Yield Nothing
		Return
	End Function

	' Token: 0x060032CC RID: 13004 RVA: 0x001D7B48 File Offset: 0x001D5F48
	Private Sub Counter()
		If Me.loopCounter < Me.maxCounter Then
			Me.loopCounter += 1
		Else
			Dim text As String = "P" + Global.UnityEngine.Random.Range(1, 5).ToStringInvariant()
			MyBase.animator.Play(text)
			Me.maxCounter = Global.UnityEngine.Random.Range(4, 6)
			Me.loopCounter = 0
		End If
	End Sub

	' Token: 0x04003AEE RID: 15086
	<SerializeField()>
	Private sprite1 As GameObject

	' Token: 0x04003AEF RID: 15087
	<SerializeField()>
	Private sprite2 As GameObject

	' Token: 0x04003AF0 RID: 15088
	<SerializeField()>
	Private sprite3 As GameObject

	' Token: 0x04003AF1 RID: 15089
	Private Const FRAME_TIME As Single = 0.083333336F

	' Token: 0x04003AF2 RID: 15090
	Private velocity As Vector2

	' Token: 0x04003AF3 RID: 15091
	Private rotation As Single

	' Token: 0x04003AF4 RID: 15092
	Private frameTime As Single

	' Token: 0x04003AF5 RID: 15093
	Private loopCounter As Integer

	' Token: 0x04003AF6 RID: 15094
	Private maxCounter As Integer

	' Token: 0x04003AF7 RID: 15095
	Private velMinMax As MinMax
End Class
