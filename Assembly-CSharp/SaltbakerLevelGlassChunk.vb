Imports System
Imports UnityEngine

' Token: 0x020007C8 RID: 1992
Public Class SaltbakerLevelGlassChunk
	Inherits AbstractMonoBehaviour

	' Token: 0x06002D2F RID: 11567 RVA: 0x001A9CB8 File Offset: 0x001A80B8
	Public Sub Reset(pos As Vector3, fallSpeed As Single, isChunk As Boolean, flip As Boolean, reverse As Boolean, inBack As Boolean, [variant] As Integer)
		MyBase.transform.position = pos
		Me.fallSpeed = fallSpeed
		MyBase.animator.SetFloat("Reverse", CSng(If((Not reverse OrElse isChunk), 1, (-1))))
		MyBase.animator.Play(If((Not isChunk), "Bit", "Chunk") + [variant].ToString(), 0, CSng(Global.UnityEngine.Random.Range(0, 1)))
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(If((Not isChunk), Global.UnityEngine.Random.Range(-30, 30), 0)))
		MyBase.transform.localScale = New Vector3(CSng(If((Not flip), 1, (-1))), 1F)
		For Each spriteRenderer As SpriteRenderer In Me.rend
			spriteRenderer.sortingLayerName = If((Not inBack), "Foreground", "Background")
			spriteRenderer.color = If((Not inBack), Color.white, New Color(0.7F, 0.7F, 0.7F, 1F))
		Next
	End Sub

	' Token: 0x06002D30 RID: 11568 RVA: 0x001A9DF7 File Offset: 0x001A81F7
	Private Sub Update()
		MyBase.transform.position += Vector3.down * Me.fallSpeed * CupheadTime.Delta
	End Sub

	' Token: 0x040035A7 RID: 13735
	Private fallSpeed As Single

	' Token: 0x040035A8 RID: 13736
	<SerializeField()>
	Private rend As SpriteRenderer()
End Class
