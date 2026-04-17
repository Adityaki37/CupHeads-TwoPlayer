Imports System
Imports UnityEngine

' Token: 0x02000B15 RID: 2837
Public Class RandomAnimation
	Inherits AbstractPausableComponent

	' Token: 0x060044C7 RID: 17607 RVA: 0x00246924 File Offset: 0x00244D24
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.SetInteger("Animation", Global.UnityEngine.Random.Range(0, MyBase.animator.GetInteger("Count")))
		MyBase.animator.speed += Global.UnityEngine.Random.Range(-Me.randomSpeed, Me.randomSpeed)
	End Sub

	' Token: 0x04004A82 RID: 19074
	<SerializeField()>
	<Range(0F, 1F)>
	Private randomSpeed As Single = 0.1F
End Class
