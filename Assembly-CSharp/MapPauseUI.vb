Imports System

' Token: 0x020009A0 RID: 2464
Public Class MapPauseUI
	Inherits LevelPauseGUI

	' Token: 0x170004B1 RID: 1201
	' (get) Token: 0x060039D1 RID: 14801 RVA: 0x0020E9F0 File Offset: 0x0020CDF0
	Protected Overrides ReadOnly Property CanPause As Boolean
		Get
			Return MyBase.state <> AbstractPauseGUI.State.Animating AndAlso MapDifficultySelectStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapConfirmStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso MapBasicStartUI.Current.CurrentState = AbstractMapSceneStartUI.State.Inactive AndAlso (SpeechBubble.Instance Is Nothing OrElse (SpeechBubble.Instance IsNot Nothing AndAlso SpeechBubble.Instance.displayState <> SpeechBubble.DisplayState.WaitForSelection)) AndAlso (Not(Map.Current IsNot Nothing) OrElse Map.Current.CurrentState <> Map.State.Graveyard)
		End Get
	End Property
End Class
