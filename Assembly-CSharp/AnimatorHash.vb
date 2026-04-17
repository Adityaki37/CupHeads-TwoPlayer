Imports System
Imports UnityEngine

' Token: 0x020002F1 RID: 753
Public Module AnimatorHash
	' Token: 0x020002F2 RID: 754
	Public NotInheritable Class level_oldman_man
		' Token: 0x020002F3 RID: 755
		Public NotInheritable Class Layer
			' Token: 0x04001149 RID: 4425
			Public Shared BaseLayer As Integer

			' Token: 0x0400114A RID: 4426
			Public Shared Beard As Integer = 1

			' Token: 0x0400114B RID: 4427
			Public Shared Eyes As Integer = 2

			' Token: 0x0400114C RID: 4428
			Public Shared GnomeA As Integer = 3

			' Token: 0x0400114D RID: 4429
			Public Shared Cauldron As Integer = 4

			' Token: 0x0400114E RID: 4430
			Public Shared GnomeB As Integer = 5
		End Class

		' Token: 0x020002F4 RID: 756
		Public NotInheritable Class ShortHash
			' Token: 0x0400114F RID: 4431
			Public Shared Idle_Part_1 As Integer = Animator.StringToHash("Idle_Part_1")

			' Token: 0x04001150 RID: 4432
			Public Shared Idle_Part_2 As Integer = Animator.StringToHash("Idle_Part_2")

			' Token: 0x04001151 RID: 4433
			Public Shared Spit_Transition_A As Integer = Animator.StringToHash("Spit_Transition_A")

			' Token: 0x04001152 RID: 4434
			Public Shared Spit_Loop As Integer = Animator.StringToHash("Spit_Loop")

			' Token: 0x04001153 RID: 4435
			Public Shared Spit_Transition_B As Integer = Animator.StringToHash("Spit_Transition_B")

			' Token: 0x04001154 RID: 4436
			Public Shared Spit_Intro_Continued As Integer = Animator.StringToHash("Spit_Intro_Continued")

			' Token: 0x04001155 RID: 4437
			Public Shared Spit_Outro As Integer = Animator.StringToHash("Spit_Outro")

			' Token: 0x04001156 RID: 4438
			Public Shared Phase_Trans As Integer = Animator.StringToHash("Phase_Trans")

			' Token: 0x04001157 RID: 4439
			Public Shared Phase_Trans_Cont As Integer = Animator.StringToHash("Phase_Trans_Cont")

			' Token: 0x04001158 RID: 4440
			Public Shared Beard_Boil As Integer = Animator.StringToHash("Beard_Boil")

			' Token: 0x04001159 RID: 4441
			Public Shared Blank As Integer = Animator.StringToHash("Blank")

			' Token: 0x0400115A RID: 4442
			Public Shared [Loop] As Integer = Animator.StringToHash("Loop")
		End Class

		' Token: 0x020002F5 RID: 757
		Public NotInheritable Class FullHash
			' Token: 0x0400115B RID: 4443
			Public Shared BaseLayer_Idle_Part_1 As Integer = Animator.StringToHash("Base Layer.Idle_Part_1")

			' Token: 0x0400115C RID: 4444
			Public Shared BaseLayer_Idle_Part_2 As Integer = Animator.StringToHash("Base Layer.Idle_Part_2")

			' Token: 0x0400115D RID: 4445
			Public Shared BaseLayer_Spit_Transition_A As Integer = Animator.StringToHash("Base Layer.Spit_Transition_A")

			' Token: 0x0400115E RID: 4446
			Public Shared BaseLayer_Spit_Loop As Integer = Animator.StringToHash("Base Layer.Spit_Loop")

			' Token: 0x0400115F RID: 4447
			Public Shared BaseLayer_Spit_Transition_B As Integer = Animator.StringToHash("Base Layer.Spit_Transition_B")

			' Token: 0x04001160 RID: 4448
			Public Shared BaseLayer_Spit_Intro_Continued As Integer = Animator.StringToHash("Base Layer.Spit_Intro_Continued")

			' Token: 0x04001161 RID: 4449
			Public Shared BaseLayer_Spit_Outro As Integer = Animator.StringToHash("Base Layer.Spit_Outro")

			' Token: 0x04001162 RID: 4450
			Public Shared BaseLayer_Phase_Trans As Integer = Animator.StringToHash("Base Layer.Phase_Trans")

			' Token: 0x04001163 RID: 4451
			Public Shared BaseLayer_Phase_Trans_Cont As Integer = Animator.StringToHash("Base Layer.Phase_Trans_Cont")

			' Token: 0x04001164 RID: 4452
			Public Shared Beard_Beard_Boil As Integer = Animator.StringToHash("Beard.Beard_Boil")

			' Token: 0x04001165 RID: 4453
			Public Shared Eyes_Blank As Integer = Animator.StringToHash("Eyes.Blank")

			' Token: 0x04001166 RID: 4454
			Public Shared Eyes_Spit_Loop As Integer = Animator.StringToHash("Eyes.Spit_Loop")

			' Token: 0x04001167 RID: 4455
			Public Shared GnomeA_Loop As Integer = Animator.StringToHash("GnomeA.Loop")

			' Token: 0x04001168 RID: 4456
			Public Shared Cauldron_Loop As Integer = Animator.StringToHash("Cauldron.Loop")

			' Token: 0x04001169 RID: 4457
			Public Shared GnomeB_Loop As Integer = Animator.StringToHash("GnomeB.Loop")
		End Class

		' Token: 0x020002F6 RID: 758
		Public NotInheritable Class Parameter
			' Token: 0x0400116A RID: 4458
			Public Shared IsSpitAttack As Integer = Animator.StringToHash("IsSpitAttack")

			' Token: 0x0400116B RID: 4459
			Public Shared IsSpitAttackEyeLoop As Integer = Animator.StringToHash("IsSpitAttackEyeLoop")

			' Token: 0x0400116C RID: 4460
			Public Shared Phase2 As Integer = Animator.StringToHash("Phase2")
		End Class
	End Class
End Module
