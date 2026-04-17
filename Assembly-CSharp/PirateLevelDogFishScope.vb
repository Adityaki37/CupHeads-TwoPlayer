Imports System

' Token: 0x02000724 RID: 1828
Public Class PirateLevelDogFishScope
	Inherits AbstractMonoBehaviour

	' Token: 0x060027CF RID: 10191 RVA: 0x00174B03 File Offset: 0x00172F03
	Public Sub [In]()
		MyBase.animator.Play("In")
	End Sub

	' Token: 0x060027D0 RID: 10192 RVA: 0x00174B15 File Offset: 0x00172F15
	Private Sub SoundDogfishPeriStart()
		AudioManager.Play("level_pirate_periscope_warning")
	End Sub
End Class
