Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007AB RID: 1963
Public Class SallyStagePlayLevelCloud
	Inherits AbstractPausableComponent

	' Token: 0x06002C1F RID: 11295 RVA: 0x0019F015 File Offset: 0x0019D415
	Private Sub Start()
		MyBase.FrameDelayedCallback(AddressOf Me.GetSprites, 1)
	End Sub

	' Token: 0x06002C20 RID: 11296 RVA: 0x0019F02C File Offset: 0x0019D42C
	Private Sub GetSprites()
		Me.normalClones = Me.normalSprite.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.shadowClones = Me.shadowSprite.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		MyBase.StartCoroutine(Me.move_all_cr())
		MyBase.StartCoroutine(Me.move_shadow_local_pos_cr())
	End Sub

	' Token: 0x06002C21 RID: 11297 RVA: 0x0019F08C File Offset: 0x0019D48C
	Private Iterator Function move_all_cr() As IEnumerator
		Dim size As Single = 500F
		Dim speed As Single = 30F
		While True
			For i As Integer = 0 To Me.normalClones.Length - 1
				If Me.normalClones(i).transform.position.x > -640F - size Then
					Me.normalClones(i).transform.position += Vector3.left * speed * CupheadTime.Delta
				Else
					Me.normalClones(i).transform.position = New Vector3(640F + size, Me.normalClones(i).transform.position.y, 0F)
				End If
			Next
			For j As Integer = 0 To Me.shadowClones.Length - 1
				If Me.shadowClones(j).transform.position.x > -640F - size Then
					Me.shadowClones(j).transform.position += Vector3.left * speed * CupheadTime.Delta
				Else
					Dim position As Vector3 = Me.shadowClones(j).transform.position
					position.x = Me.normalClones(j).transform.position.x
					Me.shadowClones(j).transform.position = position
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C22 RID: 11298 RVA: 0x0019F0A8 File Offset: 0x0019D4A8
	Private Iterator Function move_shadow_local_pos_cr() As IEnumerator
		Dim speed As Single = 1.3F
		Dim shadowOffset As Single = 5F
		While True
			For i As Integer = 0 To Me.shadowClones.Length - 1
				If Me.normalClones(i).transform.position.x > 0F AndAlso Me.normalClones(i).transform.position.x < 440F Then
					If Me.shadowClones(i).transform.position.x < Me.normalClones(i).transform.position.x + shadowOffset Then
						Me.shadowClones(i).transform.position += Vector3.right * speed * CupheadTime.Delta
					End If
				ElseIf Me.normalClones(i).transform.position.x < 0F AndAlso Me.normalClones(i).transform.position.x > -640F AndAlso Me.shadowClones(i).transform.position.x > Me.normalClones(i).transform.position.x - shadowOffset Then
					Me.shadowClones(i).transform.position -= Vector3.right * speed * CupheadTime.Delta
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040034D2 RID: 13522
	<SerializeField()>
	Private shadowSprite As SpriteRenderer

	' Token: 0x040034D3 RID: 13523
	Private shadowClones As SpriteRenderer()

	' Token: 0x040034D4 RID: 13524
	<SerializeField()>
	Private normalSprite As SpriteRenderer

	' Token: 0x040034D5 RID: 13525
	Private normalClones As SpriteRenderer()
End Class
