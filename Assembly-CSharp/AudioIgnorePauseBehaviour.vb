Imports System
Imports UnityEngine

' Token: 0x020003B6 RID: 950
Public Class AudioIgnorePauseBehaviour
	Inherits AbstractMonoBehaviour

	' Token: 0x06000BC0 RID: 3008 RVA: 0x00084BD4 File Offset: 0x00082FD4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.audioSource = MyBase.GetComponent(Of AudioSource)()
		If Me.audioSource IsNot Nothing Then
			Me.audioSource.ignoreListenerPause = True
		End If
	End Sub

	' Token: 0x04001571 RID: 5489
	Private audioSource As AudioSource
End Class
