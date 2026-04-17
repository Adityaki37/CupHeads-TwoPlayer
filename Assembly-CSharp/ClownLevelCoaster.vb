Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000561 RID: 1377
Public Class ClownLevelCoaster
	Inherits AbstractCollidableObject

	' Token: 0x060019E4 RID: 6628 RVA: 0x000EC8A3 File Offset: 0x000EACA3
	Public Sub Init(backStartPos As Vector3, frontStartPos As Vector3, properties As LevelProperties.Clown.Coaster, coasterLength As Single, coasterSize As Single, warningLights As ClownLevelLights)
		MyBase.transform.position = backStartPos
		Me.frontStartPos = frontStartPos
		Me.properties = properties
		Me.coasterLength = coasterLength
		Me.coasterSize = coasterSize
		Me.warningLights = warningLights
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x060019E5 RID: 6629 RVA: 0x000EC8E3 File Offset: 0x000EACE3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060019E6 RID: 6630 RVA: 0x000EC8F8 File Offset: 0x000EACF8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			hit.GetComponent(Of LevelPlayerController)().OnPitKnockUp(MyBase.transform.position.y, 0.85F)
		End If
	End Sub

	' Token: 0x060019E7 RID: 6631 RVA: 0x000EC944 File Offset: 0x000EAD44
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060019E8 RID: 6632 RVA: 0x000EC95C File Offset: 0x000EAD5C
	Private Sub ChompSound()
		If Me.inView Then
			AudioManager.Play("clown_coaster_main")
			Me.emitAudioFromObject.Add("clown_coaster_main")
		End If
	End Sub

	' Token: 0x060019E9 RID: 6633 RVA: 0x000EC984 File Offset: 0x000EAD84
	Private Iterator Function move_coaster_front_cr() As IEnumerator
		Dim lightsOff As Boolean = True
		AudioManager.PlayLoop("sfx_clown_coaster_ratchet_loop")
		Me.emitAudioFromObject.Add("sfx_clown_coaster_ratchet_loop")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.coasterBackToFrontDelay)
		While MyBase.transform.position.x > -640F - Me.coasterSize * Me.coasterLength
			MyBase.transform.position += -MyBase.transform.right * Me.properties.coasterSpeed * CupheadTime.Delta
			If MyBase.transform.position.x < 640F + 0.2F * Me.coasterSize AndAlso lightsOff Then
				Me.warningLights.StartWarningLights()
				lightsOff = False
			End If
			If MyBase.transform.position.x < -640F - Me.coasterSize * Me.coasterLength AndAlso Not lightsOff Then
				Me.warningLights.StopWarningLights()
				lightsOff = True
			End If
			Yield Nothing
		End While
		Me.inView = False
		AudioManager.[Stop]("sfx_clown_coaster_ratchet_loop")
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x060019EA RID: 6634 RVA: 0x000EC9A0 File Offset: 0x000EADA0
	Private Iterator Function move_coaster_back_cr() As IEnumerator
		Me.inView = True
		AudioManager.PlayLoop("sfx_clown_coaster_distant_by")
		Me.emitAudioFromObject.Add("sfx_clown_coaster_distant_by")
		While MyBase.transform.position.x < 640F + Me.coasterSize * 0.44F * Me.coasterLength
			MyBase.transform.position += MyBase.transform.right * Me.properties.coasterSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		AudioManager.[Stop]("sfx_clown_coaster_distant_by")
		Me.FrontCoasterSetup()
		Yield Nothing
		Return
	End Function

	' Token: 0x060019EB RID: 6635 RVA: 0x000EC9BC File Offset: 0x000EADBC
	Public Sub BackCoasterSetup()
		Dim num As Integer = 97
		Me.knobCollider.enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.childrenSprites = MyBase.GetComponentsInChildren(Of SpriteRenderer)()
		For Each spriteRenderer As SpriteRenderer In Me.childrenSprites
			If spriteRenderer.gameObject.GetComponent(Of LevelPlatform)() IsNot Nothing Then
				spriteRenderer.gameObject.GetComponent(Of Collider2D)().enabled = False
			End If
		Next
		MyBase.transform.SetScale(New Single?(-0.44F), New Single?(0.44F), Nothing)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(17.57F))
		Me.sprite.sortingLayerName = "Background"
		Me.sprite.sortingOrder = 45
		Dim color As Color
		ColorUtility.TryParseHtmlString("#b6b6b6", color)
		Me.sprite.color = color
		For j As Integer = 0 To Me.childrenSprites.Length - 1
			Me.childrenSprites(j).color = color
			Me.childrenSprites(j).sortingLayerName = "Background"
			Me.childrenSprites(j).sortingOrder = num - j
			If Me.childrenSprites(j).GetComponent(Of ClownLevelRiders)() IsNot Nothing Then
				Me.childrenSprites(j).GetComponent(Of Collider2D)().enabled = False
			End If
		Next
		Me.knobSprite.GetComponent(Of SpriteRenderer)().sortingLayerName = "Background"
		Me.knobSprite.GetComponent(Of SpriteRenderer)().sortingOrder = num + 1
		MyBase.StartCoroutine(Me.move_coaster_back_cr())
	End Sub

	' Token: 0x060019EC RID: 6636 RVA: 0x000ECB74 File Offset: 0x000EAF74
	Public Sub FrontCoasterSetup()
		Dim num As Integer = 79
		Me.knobCollider.enabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		For Each spriteRenderer As SpriteRenderer In Me.childrenSprites
			If spriteRenderer.gameObject.GetComponent(Of LevelPlatform)() IsNot Nothing Then
				spriteRenderer.gameObject.GetComponent(Of Collider2D)().enabled = True
			End If
		Next
		MyBase.transform.position = Me.frontStartPos
		MyBase.transform.SetScale(New Single?(1F), New Single?(1F), Nothing)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Me.sprite.sortingLayerName = "Player"
		Me.sprite.sortingOrder = 80
		Dim color As Color
		ColorUtility.TryParseHtmlString("#FFFFFFFF", color)
		Me.sprite.color = color
		For j As Integer = 0 To Me.childrenSprites.Length - 1
			Me.childrenSprites(j).color = color
			If Me.childrenSprites(j).transform.parent Is MyBase.transform OrElse Me.childrenSprites(j).transform Is MyBase.transform Then
				Me.childrenSprites(j).sortingLayerName = "Player"
				Me.childrenSprites(j).sortingOrder = num - j
			ElseIf Me.childrenSprites(j).GetComponent(Of ClownLevelRiders)() IsNot Nothing Then
				Me.childrenSprites(j).GetComponent(Of Collider2D)().enabled = True
				Me.childrenSprites(j).sortingLayerName = "Player"
				Me.childrenSprites(j).sortingOrder = num - j
				Me.childrenSprites(j).GetComponent(Of ClownLevelRiders)().FrontLayers(num - j)
			ElseIf Not Me.childrenSprites(j).transform.parent.GetComponent(Of ClownLevelRiders)() Then
				Me.childrenSprites(j).sortingLayerName = "Default"
				Me.childrenSprites(j).sortingOrder = 4
			End If
		Next
		Me.knobSprite.GetComponent(Of SpriteRenderer)().sortingLayerName = "Player"
		Me.knobSprite.GetComponent(Of SpriteRenderer)().sortingOrder = num + 1
		MyBase.StartCoroutine(Me.move_coaster_front_cr())
	End Sub

	' Token: 0x060019ED RID: 6637 RVA: 0x000ECDFB File Offset: 0x000EB1FB
	Protected Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002301 RID: 8961
	<SerializeField()>
	Private knobSprite As SpriteRenderer

	' Token: 0x04002302 RID: 8962
	<SerializeField()>
	Private knobCollider As Collider2D

	' Token: 0x04002303 RID: 8963
	Public pieceRoot As Transform

	' Token: 0x04002304 RID: 8964
	Private properties As LevelProperties.Clown.Coaster

	' Token: 0x04002305 RID: 8965
	Private warningLights As ClownLevelLights

	' Token: 0x04002306 RID: 8966
	Private sprite As SpriteRenderer

	' Token: 0x04002307 RID: 8967
	Private childrenSprites As SpriteRenderer()

	' Token: 0x04002308 RID: 8968
	Private frontStartPos As Vector3

	' Token: 0x04002309 RID: 8969
	Private damageDealer As DamageDealer

	' Token: 0x0400230A RID: 8970
	Private coasterSize As Single

	' Token: 0x0400230B RID: 8971
	Private coasterLength As Single

	' Token: 0x0400230C RID: 8972
	Private inView As Boolean
End Class
