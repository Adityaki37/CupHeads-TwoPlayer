Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000835 RID: 2101
Public Class TutorialLevelTarget
	Inherits AbstractCollidableObject

	' Token: 0x14000059 RID: 89
	' (add) Token: 0x060030B7 RID: 12471 RVA: 0x001CA654 File Offset: 0x001C8A54
	' (remove) Token: 0x060030B8 RID: 12472 RVA: 0x001CA68C File Offset: 0x001C8A8C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnShotEvent As Action

	' Token: 0x060030B9 RID: 12473 RVA: 0x001CA6C2 File Offset: 0x001C8AC2
	Protected Overrides Sub OnCollisionPlayerProjectile(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayerProjectile(hit, phase)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		If Me.OnShotEvent IsNot Nothing Then
			Me.OnShotEvent()
		End If
	End Sub
End Class
