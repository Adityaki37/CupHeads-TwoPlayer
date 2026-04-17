Imports System
Imports UnityEngine

' Token: 0x02000544 RID: 1348
Public MustInherit Class ChessLevel
	Inherits Level

	' Token: 0x060018C2 RID: 6338 RVA: 0x00057B96 File Offset: 0x00055F96
	Protected Overrides Sub Awake()
		Me.originalMode = Level.CurrentMode
		Level.SetCurrentMode(Level.Mode.Normal)
		MyBase.Awake()
	End Sub

	' Token: 0x060018C3 RID: 6339 RVA: 0x00057BAF File Offset: 0x00055FAF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Level.SetCurrentMode(Me.originalMode)
		Me.levelIntroAnimation = Nothing
	End Sub

	' Token: 0x060018C4 RID: 6340 RVA: 0x00057BC9 File Offset: 0x00055FC9
	Protected Overrides Function CreateLevelIntro(callback As Action) As LevelIntroAnimation
		Return LevelIntroAnimation.CreateCustom(Me.levelIntroAnimation, callback)
	End Function

	' Token: 0x040021CF RID: 8655
	<SerializeField()>
	Private levelIntroAnimation As LevelIntroAnimation

	' Token: 0x040021D0 RID: 8656
	Private originalMode As Level.Mode
End Class
