import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from "@angular/forms";
import { MessageService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { InputTextModule } from "primeng/inputtext";
import { InputTextareaModule } from "primeng/inputtextarea";
import { ToastModule } from "primeng/toast";

@Component({
  selector: "app-contact",
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    InputTextareaModule,
    ButtonModule,
    ToastModule,
  ],
  providers: [MessageService],
  template: `
    <div class="card">
      <h2>Contactez-nous</h2>
      <form
        [formGroup]="contactForm"
        (ngSubmit)="onSubmit()"
        class="flex flex-column gap-3"
      >
        <div class="field">
          <label for="email">Email</label>
          <input
            pInputText
            id="email"
            formControlName="email"
            type="email"
            class="w-full"
          />
          <small
            class="p-error"
            *ngIf="
              contactForm.get('email')?.invalid &&
              contactForm.get('email')?.touched
            "
          >
            L'email est requis
          </small>
        </div>

        <div class="field">
          <label for="message">Message</label>
          <textarea
            pInputTextarea
            id="message"
            formControlName="message"
            rows="5"
            class="w-full"
          ></textarea>
          <small
            class="p-error"
            *ngIf="
              contactForm.get('message')?.invalid &&
              contactForm.get('message')?.touched
            "
          >
            Le message est requis et doit faire moins de 300 caractères
          </small>
        </div>

        <p-button
          type="submit"
          label="Envoyer"
          [disabled]="contactForm.invalid"
        ></p-button>
      </form>
    </div>
    <p-toast></p-toast>
  `,
  styles: [
    `
      .card {
        padding: 2rem;
        max-width: 600px;
        margin: 2rem auto;
      }
      .field {
        margin-bottom: 1rem;
      }
    `,
  ],
})
export class ContactComponent {
  contactForm: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly messageService: MessageService
  ) {
    this.contactForm = this.fb.group({
      email: ["", [Validators.required, Validators.email]],
      message: ["", [Validators.required, Validators.maxLength(300)]],
    });
  }

  onSubmit() {
    if (this.contactForm.valid) {
      // Log the email and message to the console
      console.log("Email:", this.contactForm.value.email);
      console.log("Message:", this.contactForm.value.message);

      // Afficher le message de succès
      this.messageService.add({
        severity: "success",
        summary: "Succès",
        detail: "Demande de contact envoyée avec succès",
      });
      this.contactForm.reset();
    }
  }
}
