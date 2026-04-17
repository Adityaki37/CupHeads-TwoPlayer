Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B05 RID: 2821
Public Class ShopSceneBuyCoin
	Inherits MonoBehaviour

	' Token: 0x0600446C RID: 17516 RVA: 0x002434D8 File Offset: 0x002418D8
	Private Sub Start()
		Me.velocity = New Vector2(Global.UnityEngine.Random.Range(Me.VelocityXMin, Me.VelocityXMax), Global.UnityEngine.Random.Range(Me.VelocityYMin, Me.VelocityYMax))
		Me.randomRotation = New Vector2(CSng(Global.UnityEngine.Random.Range(-500, 500)), CSng(Global.UnityEngine.Random.Range(-500, 500)))
		MyBase.StartCoroutine(Me.scaledown_cr())
	End Sub

	' Token: 0x0600446D RID: 17517 RVA: 0x0024354C File Offset: 0x0024194C
	Private Sub Update()
		MyBase.transform.position += (Me.velocity + New Vector2(-300F, Me.accumulatedGravity)) * Time.fixedDeltaTime
		Me.accumulatedGravity += -100F
		MyBase.transform.Rotate(Me.randomRotation * Time.deltaTime)
	End Sub

	' Token: 0x0600446E RID: 17518 RVA: 0x002435CC File Offset: 0x002419CC
	Private Iterator Function scaledown_cr() As IEnumerator
		Dim startScale As Vector2 = MyBase.transform.localScale
		Dim t As Single = 0F
		Dim TIME As Single = 1F
		While t < TIME
			Dim val As Single = t / TIME
			Dim newAlpha As Single = Mathf.Lerp(1F, 0F, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, val))
			Dim newColor As Color = Me.spriteRenderer.color
			newColor.a = newAlpha
			Me.spriteRenderer.color = newColor
			t += Time.deltaTime
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0600446F RID: 17519 RVA: 0x002435E7 File Offset: 0x002419E7
	Private Sub OnDestroy()
		Me.spriteRenderer = Nothing
	End Sub

	' Token: 0x04004A06 RID: 18950
	Public VelocityXMin As Single = -500F

	' Token: 0x04004A07 RID: 18951
	Public VelocityXMax As Single = 500F

	' Token: 0x04004A08 RID: 18952
	Public VelocityYMin As Single = 500F

	' Token: 0x04004A09 RID: 18953
	Public VelocityYMax As Single = 1000F

	' Token: 0x04004A0A RID: 18954
	Private Const GRAVITY As Single = -100F

	' Token: 0x04004A0B RID: 18955
	Private velocity As Vector2

	' Token: 0x04004A0C RID: 18956
	Private randomRotation As Vector2

	' Token: 0x04004A0D RID: 18957
	Private accumulatedGravity As Single

	' Token: 0x04004A0E RID: 18958
	<SerializeField()>
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04004A0F RID: 18959
	<SerializeField()>
	Private isCoinA As Boolean
End Class
