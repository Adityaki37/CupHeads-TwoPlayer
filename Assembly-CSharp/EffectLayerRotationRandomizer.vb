Imports System
Imports UnityEngine

' Token: 0x02000B0F RID: 2831
Public Class EffectLayerRotationRandomizer
	Inherits MonoBehaviour

	' Token: 0x060044B4 RID: 17588 RVA: 0x00246240 File Offset: 0x00244640
	Private Sub Awake()
		If Me.randomizeRotation Then
			MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		End If
		MyBase.transform.localScale = New Vector3(If((Not Me.randomizeXFlip), MyBase.transform.localScale.x, CSng(MathUtils.PlusOrMinus())), If((Not Me.randomizeYFlip), MyBase.transform.localScale.y, CSng(MathUtils.PlusOrMinus())))
		MyBase.enabled = False
	End Sub

	' Token: 0x04004A6C RID: 19052
	<SerializeField()>
	Private randomizeRotation As Boolean = True

	' Token: 0x04004A6D RID: 19053
	<SerializeField()>
	Private randomizeXFlip As Boolean = True

	' Token: 0x04004A6E RID: 19054
	<SerializeField()>
	Private randomizeYFlip As Boolean = True
End Class
