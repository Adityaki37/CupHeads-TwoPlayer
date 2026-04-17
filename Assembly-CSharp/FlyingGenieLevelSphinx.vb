Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200067B RID: 1659
Public Class FlyingGenieLevelSphinx
	Inherits AbstractProjectile

	' Token: 0x060022F9 RID: 8953 RVA: 0x001483B1 File Offset: 0x001467B1
	Public Sub Init(startPos As Vector3, properties As LevelProperties.FlyingGenie.Sphinx, player As AbstractPlayerController, pinkPattern As String(), pinkIndex As Integer)
		MyBase.transform.position = startPos
		Me.properties = properties
		Me.player = player
		Me.pinkPattern = pinkPattern
		Me.pinkIndex = pinkIndex
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060022FA RID: 8954 RVA: 0x001483EA File Offset: 0x001467EA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060022FB RID: 8955 RVA: 0x00148413 File Offset: 0x00146813
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060022FC RID: 8956 RVA: 0x00148434 File Offset: 0x00146834
	Private Iterator Function move_cr() As IEnumerator
		Dim startPos As Single = (MyBase.transform.position + Vector3.up * Me.outOfChestY).y
		While MyBase.transform.position.y < startPos
			MyBase.transform.AddPosition(0F, Me.outOfChestY * Me.outOfChestSpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Me.sphinxRenderer.sortingLayerName = "Projectiles"
		Me.sphinxRenderer.sortingOrder = 2
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim targetPos As Vector3 = Me.player.transform.position
		While MyBase.transform.position <> targetPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, targetPos, Me.properties.sphinxSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.splitDelay)
		MyBase.animator.SetTrigger("Split")
		Return
	End Function

	' Token: 0x060022FD RID: 8957 RVA: 0x0014844F File Offset: 0x0014684F
	Public Sub Split()
		MyBase.StartCoroutine(Me.split_cr())
		AudioManager.Play("genie_scarab_release")
		Me.emitAudioFromObject.Add("genie_scarab_release")
	End Sub

	' Token: 0x060022FE RID: 8958 RVA: 0x00148478 File Offset: 0x00146878
	Private Iterator Function split_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Dim counter As Integer = CInt(Mathf.Round(Me.properties.sphinxSpawnNum / 2F))
		Dim moveRight As Boolean = False
		For i As Integer = 0 To Me.sphinxPieces.Length - 1
			If Me.player Is Nothing OrElse Me.player.IsDead Then
				Me.player = PlayerManager.GetNext()
			End If
			Dim pink As Integer = (Me.pinkIndex + i * 2) Mod Me.pinkPattern.Length
			Me.sphinxPieces(i).StartMoving(Me.properties, Me.player, counter, moveRight, Me.pinkPattern, pink)
			moveRight = Not moveRight
			Yield Nothing
		Next
		Me.Die()
		While MyBase.transform.childCount > 1
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060022FF RID: 8959 RVA: 0x00148493 File Offset: 0x00146893
	Protected Overrides Sub Die()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x04002B91 RID: 11153
	Private Const SplitParameterName As String = "Split"

	' Token: 0x04002B92 RID: 11154
	Private Const ProjectilesLayer As String = "Projectiles"

	' Token: 0x04002B93 RID: 11155
	<SerializeField()>
	Private sphinxRenderer As SpriteRenderer

	' Token: 0x04002B94 RID: 11156
	<SerializeField()>
	Private outOfChestY As Single

	' Token: 0x04002B95 RID: 11157
	<SerializeField()>
	Private outOfChestSpeed As Single

	' Token: 0x04002B96 RID: 11158
	Public sphinxPieces As FlyingGenieLevelSphinxPiece()

	' Token: 0x04002B97 RID: 11159
	Private player As AbstractPlayerController

	' Token: 0x04002B98 RID: 11160
	Private properties As LevelProperties.FlyingGenie.Sphinx

	' Token: 0x04002B99 RID: 11161
	Private moving As Boolean

	' Token: 0x04002B9A RID: 11162
	Private pinkPattern As String()

	' Token: 0x04002B9B RID: 11163
	Private pinkIndex As Integer
End Class
