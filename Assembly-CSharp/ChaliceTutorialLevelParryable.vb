Imports System
Imports UnityEngine

' Token: 0x02000527 RID: 1319
Public Class ChaliceTutorialLevelParryable
	Inherits ParrySwitch

	' Token: 0x1700032D RID: 813
	' (get) Token: 0x060017BF RID: 6079 RVA: 0x000D5E06 File Offset: 0x000D4206
	' (set) Token: 0x060017C0 RID: 6080 RVA: 0x000D5E0E File Offset: 0x000D420E
	Public Property isDeactivated As Boolean

	' Token: 0x060017C1 RID: 6081 RVA: 0x000D5E17 File Offset: 0x000D4217
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.Deactivated()
	End Sub

	' Token: 0x060017C2 RID: 6082 RVA: 0x000D5E25 File Offset: 0x000D4225
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		Me.Deactivated()
	End Sub

	' Token: 0x060017C3 RID: 6083 RVA: 0x000D5E34 File Offset: 0x000D4234
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		AudioManager.Play("sfx_parry_pink_shows")
	End Sub

	' Token: 0x060017C4 RID: 6084 RVA: 0x000D5E47 File Offset: 0x000D4247
	Public Sub Deactivated()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.isDeactivated = True
	End Sub

	' Token: 0x060017C5 RID: 6085 RVA: 0x000D5E5C File Offset: 0x000D425C
	Public Sub Activated()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.isDeactivated = False
	End Sub

	' Token: 0x060017C6 RID: 6086 RVA: 0x000D5E74 File Offset: 0x000D4274
	Private Sub Update()
		Me.rend.color = New Color(1F, 1F, 1F, Mathf.Clamp(Me.rend.color.a + If((Not Me.isDeactivated), CupheadTime.Delta, (-CupheadTime.Delta)) * 5F, 0F, 1F))
	End Sub

	' Token: 0x040020EB RID: 8427
	Private Const FADE_SPEED As Single = 5F

	' Token: 0x040020EC RID: 8428
	<SerializeField()>
	Private firstOne As Boolean

	' Token: 0x040020ED RID: 8429
	<SerializeField()>
	Private rend As SpriteRenderer
End Class
