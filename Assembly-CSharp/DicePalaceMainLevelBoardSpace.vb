Imports System
Imports UnityEngine

' Token: 0x020005CF RID: 1487
Public Class DicePalaceMainLevelBoardSpace
	Inherits MonoBehaviour

	' Token: 0x17000366 RID: 870
	' (get) Token: 0x06001D35 RID: 7477 RVA: 0x0010C476 File Offset: 0x0010A876
	' (set) Token: 0x06001D36 RID: 7478 RVA: 0x0010C4A7 File Offset: 0x0010A8A7
	Public Property HasHeart As Boolean
		Get
			Return Not(Me.heartSpace Is Nothing) AndAlso Not(Me.odds Is Nothing) AndAlso Me.heartSpace.activeSelf
		End Get
		Set(value As Boolean)
			If Me.heartSpace IsNot Nothing AndAlso Me.odds IsNot Nothing Then
				Me.odds.SetActive(Not value)
				Me.heartSpace.SetActive(value)
			End If
		End Set
	End Property

	' Token: 0x17000367 RID: 871
	' (get) Token: 0x06001D37 RID: 7479 RVA: 0x0010C4E6 File Offset: 0x0010A8E6
	Public ReadOnly Property Pivot As Transform
		Get
			Return Me.pivot
		End Get
	End Property

	' Token: 0x17000368 RID: 872
	' (set) Token: 0x06001D38 RID: 7480 RVA: 0x0010C4EE File Offset: 0x0010A8EE
	Public WriteOnly Property Clear As Boolean
		Set(value As Boolean)
			Me.space.SetActive(Not value)
			Me.clearSpace.SetActive(value)
		End Set
	End Property

	' Token: 0x17000369 RID: 873
	' (get) Token: 0x06001D39 RID: 7481 RVA: 0x0010C50B File Offset: 0x0010A90B
	Public ReadOnly Property HeartSpacePosition As Vector3
		Get
			If Me.heartSpace IsNot Nothing Then
				Return Me.heartSpace.transform.position
			End If
			Return Vector3.zero
		End Get
	End Property

	' Token: 0x0400261F RID: 9759
	<SerializeField()>
	Private space As GameObject

	' Token: 0x04002620 RID: 9760
	<SerializeField()>
	Private clearSpace As GameObject

	' Token: 0x04002621 RID: 9761
	<SerializeField()>
	Private odds As GameObject

	' Token: 0x04002622 RID: 9762
	<SerializeField()>
	Private heartSpace As GameObject

	' Token: 0x04002623 RID: 9763
	<SerializeField()>
	Private pivot As Transform
End Class
