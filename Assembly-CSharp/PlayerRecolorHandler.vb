Imports System
Imports UnityEngine

' Token: 0x02000AD2 RID: 2770
Public Class PlayerRecolorHandler
	Inherits MonoBehaviour

	' Token: 0x06004288 RID: 17032 RVA: 0x0023DD4C File Offset: 0x0023C14C
	Private Sub OnEnable()
		EventManager.Instance.AddListener(Of ChaliceRecolorEvent)(AddressOf Me.chaliceRecolorEventHandler)
	End Sub

	' Token: 0x06004289 RID: 17033 RVA: 0x0023DD64 File Offset: 0x0023C164
	Private Sub OnDisable()
		EventManager.Instance.RemoveListener(Of ChaliceRecolorEvent)(AddressOf Me.chaliceRecolorEventHandler)
	End Sub

	' Token: 0x0600428A RID: 17034 RVA: 0x0023DD7C File Offset: 0x0023C17C
	Private Sub chaliceRecolorEventHandler(e As ChaliceRecolorEvent)
		PlayerRecolorHandler.SetChaliceRecolorEnabled(Me.chaliceRenderer.sharedMaterial, e.enabled)
	End Sub

	' Token: 0x0600428B RID: 17035 RVA: 0x0023DD94 File Offset: 0x0023C194
	Public Shared Sub SetChaliceRecolorEnabled(sharedMaterial As Material, enabled As Boolean)
		sharedMaterial.SetFloat("_RecolorFactor", CSng(If((Not enabled), 0, 1)))
	End Sub

	' Token: 0x040048F3 RID: 18675
	<SerializeField()>
	Private chaliceRenderer As SpriteRenderer
End Class
