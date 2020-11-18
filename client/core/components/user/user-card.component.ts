import {
  Component,
  Input
} from '@angular/core';

import { User } from '../../models';

@Component({
  selector: 'user-card',
  templateUrl: 'user-card.component.html'
})
export class UserCardComponent {
  @Input() size = 420;
  @Input() user: User;
}
