Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009B1 RID: 2481
Public Class SplashScreenMDHR
	Inherits AbstractMonoBehaviour

	' Token: 0x06003A30 RID: 14896 RVA: 0x0021155A File Offset: 0x0020F95A
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Cuphead.Init(False)
		Me.input = New CupheadInput.AnyPlayerInput(False)
		Me.fader.color = New Color(0F, 0F, 0F, 1F)
	End Sub

	' Token: 0x06003A31 RID: 14897 RVA: 0x00211598 File Offset: 0x0020F998
	Private Sub Start()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x06003A32 RID: 14898 RVA: 0x002115A7 File Offset: 0x0020F9A7
	Private Sub Update()
		If Me.fading Then
			Return
		End If
		If Me.input.GetButtonDown(CupheadButton.Accept) Then
			Me.BeginFadeOut()
		End If
	End Sub

	' Token: 0x06003A33 RID: 14899 RVA: 0x002115D0 File Offset: 0x0020F9D0
	Private Iterator Function go_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		MyBase.animator.Play("Logo")
		Return
	End Function

	' Token: 0x06003A34 RID: 14900 RVA: 0x002115EB File Offset: 0x0020F9EB
	Private Sub BeginFadeOut()
		If Me.fading Then
			Return
		End If
		Me.fading = True
		SceneLoader.properties.transitionEnd = SceneLoader.Transition.Iris
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Iris
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
	End Sub

	' Token: 0x04004268 RID: 17000
	Private Const WAIT As Single = 3F

	' Token: 0x04004269 RID: 17001
	<SerializeField()>
	Private fader As SpriteRenderer

	' Token: 0x0400426A RID: 17002
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x0400426B RID: 17003
	Private fading As Boolean
End Class
