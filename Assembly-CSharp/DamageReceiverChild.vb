Imports System
Imports UnityEngine

' Token: 0x02000AF5 RID: 2805
Public Class DamageReceiverChild
	Inherits AbstractMonoBehaviour

	' Token: 0x17000614 RID: 1556
	' (get) Token: 0x06004401 RID: 17409 RVA: 0x002409FE File Offset: 0x0023EDFE
	Public ReadOnly Property Receiver As DamageReceiver
		Get
			Return Me.receiver
		End Get
	End Property

	' Token: 0x06004402 RID: 17410 RVA: 0x00240A06 File Offset: 0x0023EE06
	Private Sub Start()
		MyBase.tag = Me.receiver.tag
	End Sub

	' Token: 0x0400499D RID: 18845
	<SerializeField()>
	Private receiver As DamageReceiver
End Class
