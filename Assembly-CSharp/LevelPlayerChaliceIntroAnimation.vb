Imports System
Imports UnityEngine

' Token: 0x02000A18 RID: 2584
Public Class LevelPlayerChaliceIntroAnimation
	Inherits Effect

	' Token: 0x06003D42 RID: 15682 RVA: 0x0021E410 File Offset: 0x0021C810
	Public Function Create(position As Vector3, isMugman As Boolean, isScared As Boolean) As LevelPlayerChaliceIntroAnimation
		Dim levelPlayerChaliceIntroAnimation As LevelPlayerChaliceIntroAnimation = TryCast(MyBase.Create(position), LevelPlayerChaliceIntroAnimation)
		levelPlayerChaliceIntroAnimation.SetSprites(isMugman)
		Dim text As String = "Intro_CH_MM" + If((Not isScared), "_Hold", "_Scared")
		levelPlayerChaliceIntroAnimation.animator.Play(text)
		Return levelPlayerChaliceIntroAnimation
	End Function

	' Token: 0x06003D43 RID: 15683 RVA: 0x0021E45E File Offset: 0x0021C85E
	Public Sub EndHold()
		MyBase.animator.SetTrigger("Continue")
	End Sub

	' Token: 0x06003D44 RID: 15684 RVA: 0x0021E470 File Offset: 0x0021C870
	Private Sub SetSprites(isMugman As Boolean)
		If Level.Current.CurrentLevel = Levels.Saltbaker Then
			Me.cuphead.SetActive(False)
			Me.mugman.SetActive(False)
		Else
			Me.cuphead.SetActive(Not isMugman)
			Me.mugman.SetActive(isMugman)
		End If
	End Sub

	' Token: 0x04004492 RID: 17554
	<SerializeField()>
	Private cuphead As GameObject

	' Token: 0x04004493 RID: 17555
	<SerializeField()>
	Private mugman As GameObject
End Class
