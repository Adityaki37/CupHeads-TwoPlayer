Imports System

' Token: 0x02000AE2 RID: 2786
Public Class PlmManager
	' Token: 0x170005F2 RID: 1522
	' (get) Token: 0x06004335 RID: 17205 RVA: 0x0023FCE7 File Offset: 0x0023E0E7
	Public Shared ReadOnly Property Instance As PlmManager
		Get
			If PlmManager.instance Is Nothing Then
				PlmManager.instance = New PlmManager()
			End If
			Return PlmManager.instance
		End Get
	End Property

	' Token: 0x170005F3 RID: 1523
	' (get) Token: 0x06004336 RID: 17206 RVA: 0x0023FD02 File Offset: 0x0023E102
	' (set) Token: 0x06004337 RID: 17207 RVA: 0x0023FD0A File Offset: 0x0023E10A
	Public Property [Interface] As PlmInterface

	' Token: 0x06004338 RID: 17208 RVA: 0x0023FD14 File Offset: 0x0023E114
	Public Sub Init()
		Me.[Interface] = New DummyPlmInterface()
		Me.[Interface].Init()
		AddHandler Me.[Interface].OnSuspend, AddressOf Me.OnSuspend
		AddHandler Me.[Interface].OnResume, AddressOf Me.OnResume
		AddHandler Me.[Interface].OnConstrained, AddressOf Me.OnConstrained
		AddHandler Me.[Interface].OnUnconstrained, AddressOf Me.OnUnconstrained
	End Sub

	' Token: 0x06004339 RID: 17209 RVA: 0x0023FD93 File Offset: 0x0023E193
	Private Sub OnSuspend()
	End Sub

	' Token: 0x0600433A RID: 17210 RVA: 0x0023FD95 File Offset: 0x0023E195
	Private Sub OnResume()
	End Sub

	' Token: 0x0600433B RID: 17211 RVA: 0x0023FD97 File Offset: 0x0023E197
	Private Sub OnConstrained()
	End Sub

	' Token: 0x0600433C RID: 17212 RVA: 0x0023FD99 File Offset: 0x0023E199
	Private Sub OnUnconstrained()
	End Sub

	' Token: 0x04004929 RID: 18729
	Private Shared instance As PlmManager
End Class
