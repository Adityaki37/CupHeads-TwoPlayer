Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000907 RID: 2311
Public Class PlatformingLevelParryablePlatform
	Inherits ParrySwitch

	' Token: 0x17000465 RID: 1125
	' (get) Token: 0x0600363C RID: 13884 RVA: 0x001F7963 File Offset: 0x001F5D63
	' (set) Token: 0x0600363D RID: 13885 RVA: 0x001F7970 File Offset: 0x001F5D70
	Public Property enabled As Boolean
		Get
			Return MyBase.GetComponent(Of Collider2D)().enabled
		End Get
		Set(value As Boolean)
			MyBase.GetComponent(Of Collider2D)().enabled = value
		End Set
	End Property

	' Token: 0x0600363E RID: 13886 RVA: 0x001F7980 File Offset: 0x001F5D80
	Private Sub Start()
		Me.platform.SetActive(False)
		Me.platform.transform.SetScale(New Single?(Me.platformWidth), New Single?(5F), New Single?(5F))
		Me.pink = MyBase.GetComponent(Of SpriteRenderer)().color
	End Sub

	' Token: 0x0600363F RID: 13887 RVA: 0x001F79DC File Offset: 0x001F5DDC
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		Me.platform.SetActive(True)
		Me.enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06003640 RID: 13888 RVA: 0x001F7A30 File Offset: 0x001F5E30
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.openDuration)
		Me.platform.SetActive(False)
		Me.enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().color = Me.pink
		Return
	End Function

	' Token: 0x04003E36 RID: 15926
	<SerializeField()>
	Private platform As GameObject

	' Token: 0x04003E37 RID: 15927
	<SerializeField()>
	Private openDuration As Single = 3F

	' Token: 0x04003E38 RID: 15928
	<SerializeField()>
	Private platformWidth As Single = 36F

	' Token: 0x04003E39 RID: 15929
	Private pink As Color
End Class
