Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009C8 RID: 2504
Public Class AbstractPausableComponent
	Inherits AbstractMonoBehaviour

	' Token: 0x170004D2 RID: 1234
	' (get) Token: 0x06003ADC RID: 15068 RVA: 0x0000836A File Offset: 0x0000676A
	Protected Overridable ReadOnly Property emitTransform As Transform
		Get
			Return MyBase.transform
		End Get
	End Property

	' Token: 0x06003ADD RID: 15069 RVA: 0x00008372 File Offset: 0x00006772
	Protected Overrides Sub Awake()
		MyBase.Awake()
		PauseManager.AddChild(Me)
		Me.preEnabled = MyBase.enabled
		Me.emitAudioFromObject = New SoundEmitter(Me)
	End Sub

	' Token: 0x06003ADE RID: 15070 RVA: 0x00008398 File Offset: 0x00006798
	Protected Overridable Sub OnDestroy()
		PauseManager.RemoveChild(Me)
	End Sub

	' Token: 0x06003ADF RID: 15071 RVA: 0x000083A0 File Offset: 0x000067A0
	Public Overridable Sub OnPause()
	End Sub

	' Token: 0x06003AE0 RID: 15072 RVA: 0x000083A2 File Offset: 0x000067A2
	Public Overridable Sub OnUnpause()
	End Sub

	' Token: 0x06003AE1 RID: 15073 RVA: 0x000083A4 File Offset: 0x000067A4
	Protected Iterator Function WaitForPause_CR() As IEnumerator
		While PauseManager.state = PauseManager.State.Paused
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003AE2 RID: 15074 RVA: 0x000083B8 File Offset: 0x000067B8
	Public Overridable Sub OnLevelEnd()
		If Me IsNot Nothing Then
			Me.StopAllCoroutines()
			MyBase.enabled = False
		End If
	End Sub

	' Token: 0x06003AE3 RID: 15075 RVA: 0x000083D3 File Offset: 0x000067D3
	Public Sub EmitSound(key As String)
		AudioManager.FollowObject(key, Me.emitTransform)
	End Sub

	' Token: 0x040042A1 RID: 17057
	<NonSerialized()>
	Public preEnabled As Boolean

	' Token: 0x040042A2 RID: 17058
	Protected emitAudioFromObject As SoundEmitter
End Class
