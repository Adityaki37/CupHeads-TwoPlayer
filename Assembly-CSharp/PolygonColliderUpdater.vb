Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020003EB RID: 1003
Public Class PolygonColliderUpdater
	Inherits AbstractMonoBehaviour

	' Token: 0x06000D8A RID: 3466 RVA: 0x0008E1C8 File Offset: 0x0008C5C8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.colliders = New Dictionary(Of String, PolygonCollider2D)()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Me.sprite = Nothing
		If MyBase.GetComponent(Of Collider2D)() IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(MyBase.GetComponent(Of Collider2D)())
		End If
		MyBase.StartCoroutine(Me.collider_cr())
	End Sub

	' Token: 0x06000D8B RID: 3467 RVA: 0x0008E224 File Offset: 0x0008C624
	Private Iterator Function collider_cr() As IEnumerator
		While True
			If Me.spriteRenderer.sprite IsNot Me.sprite Then
				Me.sprite = Me.spriteRenderer.sprite
				For Each polygonCollider2D As PolygonCollider2D In MyBase.GetComponents(Of PolygonCollider2D)()
					polygonCollider2D.enabled = False
				Next
				If Me.colliders.ContainsKey(Me.sprite.name) Then
					Me.colliders(Me.sprite.name).enabled = True
				Else
					Me.colliders(Me.sprite.name) = MyBase.gameObject.AddComponent(Of PolygonCollider2D)()
					Me.colliders(Me.sprite.name).isTrigger = True
				End If
			End If
			Yield New WaitForEndOfFrame()
		End While
		Return
	End Function

	' Token: 0x04001703 RID: 5891
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04001704 RID: 5892
	Private sprite As Sprite

	' Token: 0x04001705 RID: 5893
	Private colliders As Dictionary(Of String, PolygonCollider2D)
End Class
