Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020009CA RID: 2506
Public Module PauseManager
	' Token: 0x06003AE6 RID: 15078 RVA: 0x0021258A File Offset: 0x0021098A
	Public Sub Reset()
		PauseManager.state = PauseManager.State.Unpaused
	End Sub

	' Token: 0x06003AE7 RID: 15079 RVA: 0x00212592 File Offset: 0x00210992
	Public Sub AddChild(child As AbstractPausableComponent)
		PauseManager.children.Add(child)
	End Sub

	' Token: 0x06003AE8 RID: 15080 RVA: 0x0021259F File Offset: 0x0021099F
	Public Sub RemoveChild(child As AbstractPausableComponent)
		PauseManager.children.Remove(child)
	End Sub

	' Token: 0x06003AE9 RID: 15081 RVA: 0x002125B0 File Offset: 0x002109B0
	Public Sub Pause()
		If PauseManager.state = PauseManager.State.Paused Then
			Return
		End If
		PauseManager.state = PauseManager.State.Paused
		AudioListener.pause = True
		PauseManager.oldSpeed = CupheadTime.GlobalSpeed
		CupheadTime.GlobalSpeed = 0F
		For Each abstractPausableComponent As AbstractPausableComponent In PauseManager.children
			abstractPausableComponent.OnPause()
		Next
		PauseManager.SetChildren(False)
	End Sub

	' Token: 0x06003AEA RID: 15082 RVA: 0x0021263C File Offset: 0x00210A3C
	Public Sub Unpause()
		If PauseManager.state = PauseManager.State.Unpaused Then
			Return
		End If
		PauseManager.state = PauseManager.State.Unpaused
		AudioListener.pause = False
		CupheadTime.GlobalSpeed = PauseManager.oldSpeed
		For Each abstractPausableComponent As AbstractPausableComponent In PauseManager.children
			abstractPausableComponent.OnUnpause()
		Next
		PauseManager.SetChildren(True)
	End Sub

	' Token: 0x06003AEB RID: 15083 RVA: 0x002126C0 File Offset: 0x00210AC0
	Public Sub Toggle()
		If PauseManager.state = PauseManager.State.Paused Then
			PauseManager.Unpause()
		Else
			PauseManager.Pause()
		End If
	End Sub

	' Token: 0x06003AEC RID: 15084 RVA: 0x002126DC File Offset: 0x00210ADC
	Private Sub SetChildren(enabled As Boolean)
		For i As Integer = 0 To PauseManager.children.Count - 1
			Dim abstractPausableComponent As AbstractPausableComponent = PauseManager.children(i)
			If abstractPausableComponent Is Nothing Then
				PauseManager.children.Remove(abstractPausableComponent)
				i -= 1
			ElseIf enabled Then
				abstractPausableComponent.enabled = abstractPausableComponent.preEnabled
			Else
				abstractPausableComponent.preEnabled = abstractPausableComponent.enabled
				abstractPausableComponent.enabled = False
			End If
		Next
	End Sub

	' Token: 0x040042A4 RID: 17060
	Public state As PauseManager.State

	' Token: 0x040042A5 RID: 17061
	Private oldSpeed As Single

	' Token: 0x040042A6 RID: 17062
	Private children As List(Of AbstractPausableComponent) = New List(Of AbstractPausableComponent)()

	' Token: 0x020009CB RID: 2507
	Public Enum State
		' Token: 0x040042A8 RID: 17064
		Unpaused
		' Token: 0x040042A9 RID: 17065
		Paused
	End Enum
End Module
