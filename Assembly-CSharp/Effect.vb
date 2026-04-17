Imports System
Imports UnityEngine

' Token: 0x02000B0E RID: 2830
<Serializable()>
Public Class Effect
	Inherits AbstractCollidableObject

	' Token: 0x060044AC RID: 17580 RVA: 0x000B9EAB File Offset: 0x000B82AB
	Public Overridable Function Create(position As Vector3) As Effect
		Return Me.Create(position, Vector3.one)
	End Function

	' Token: 0x060044AD RID: 17581 RVA: 0x000B9EBC File Offset: 0x000B82BC
	Public Overridable Function Create(position As Vector3, scale As Vector3) As Effect
		Dim component As Effect = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject).GetComponent(Of Effect)()
		component.name = component.name.Replace("(Clone)", String.Empty)
		If Me.randomMirrorX Then
			scale.x = If((Not Rand.Bool()), (-scale.x), scale.x)
		End If
		If Me.randomMirrorY Then
			scale.y = If((Not Rand.Bool()), (-scale.y), scale.y)
		End If
		component.Initialize(position, scale, Me.randomRotation)
		Return component
	End Function

	' Token: 0x060044AE RID: 17582 RVA: 0x000B9F64 File Offset: 0x000B8364
	Public Overridable Sub Initialize(position As Vector3)
		Dim vector As Vector3 = New Vector3(1F, 1F)
		If Me.randomMirrorX Then
			vector.x = If((Not Rand.Bool()), (-vector.x), vector.x)
		End If
		If Me.randomMirrorY Then
			vector.y = If((Not Rand.Bool()), (-vector.y), vector.y)
		End If
		Me.Initialize(position, vector, Me.randomRotation)
	End Sub

	' Token: 0x060044AF RID: 17583 RVA: 0x000B9FF0 File Offset: 0x000B83F0
	Public Overridable Sub Initialize(position As Vector3, scale As Vector3, randomR As Boolean)
		Dim num As Integer = Global.UnityEngine.Random.Range(0, MyBase.animator.GetInteger("Count"))
		MyBase.animator.SetInteger("Effect", num)
		Dim transform As Transform = MyBase.transform
		transform.position = position
		transform.localScale = scale
		If randomR Then
			transform.eulerAngles = New Vector3(0F, 0F, Global.UnityEngine.Random.Range(0F, 360F))
		End If
	End Sub

	' Token: 0x060044B0 RID: 17584 RVA: 0x000BA064 File Offset: 0x000B8464
	Protected Overridable Sub OnEffectCompletePool()
		If Me.removeOnEnd Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Else
			Me.inUse = False
		End If
	End Sub

	' Token: 0x060044B1 RID: 17585 RVA: 0x000BA088 File Offset: 0x000B8488
	Protected Overridable Sub OnEffectComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060044B2 RID: 17586 RVA: 0x000BA095 File Offset: 0x000B8495
	Public Sub Play()
		MyBase.animator.Play("A")
	End Sub

	' Token: 0x04004A67 RID: 19047
	<SerializeField()>
	Protected randomRotation As Boolean

	' Token: 0x04004A68 RID: 19048
	<Space(10F)>
	<SerializeField()>
	Protected randomMirrorX As Boolean

	' Token: 0x04004A69 RID: 19049
	<SerializeField()>
	Protected randomMirrorY As Boolean

	' Token: 0x04004A6A RID: 19050
	Public inUse As Boolean

	' Token: 0x04004A6B RID: 19051
	Public removeOnEnd As Boolean
End Class
