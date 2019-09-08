<template>
  <div
    id="page-content"
    class="faq"
  >
    <h2>FAQ</h2>
    <ul>
      <li class="question-item">
        <h3 class="question">What is Scholars Roundtable?</h3>
        <p class="answer">A definitive source for ranking Bible commentaries based on input from the most qualified and trusted biblical scholars.</p>
      </li>
      <li class="question-item">
        <h3 class="question">Who are your contributors?</h3>
        <p class="answer">Our list of contributors is growing day by day, but some of our contributors include scholars such as William Barrick, Duane Garrett, Thomas Schreiner, Mark Rooker, Robert McCabe, Andy Naselli, Jason DeRouchie, Patrick Schreiner, Matt Waymeyer, Benjamin Gladd, Paul Wegner, Jan Verbruggen, Brian Gault, Andrew King, Darian Lockett, Brian Vickers, Brian Tabb, Peter Gentry, Michael Barrett, Charles Quarles, Stephen Andrews, Josh Mathews, George Zemek, and George Guthrie among others. Our contributors have significant ties to institutions such as <strong>Southern Baptist Theological Seminary</strong>, <strong>Western Seminary</strong>, <strong>Columbia Biblical Seminary</strong>, <strong>The Master’s Seminary</strong>, <strong>Gateway Seminary</strong>, <strong>Puritan Reformed Theological Seminary</strong>, <strong>Dallas Baptist Theological Seminary</strong>, <strong>Bethlehem College and Seminary</strong>, <strong>Talbot School of Theology at Biola</strong>, <strong>Midwestern Baptist Theological Seminary</strong>, <strong>Southeastern Baptist Theological Seminary</strong>, <strong>Regent College</strong>, <strong>Reformed Theological Seminary</strong>, and <strong>Expositor’s Seminary</strong>.</p>
      </li>
      <li class="question-item">
        <h3 class="question">What information do the contributors give you?</h3>
        <p class="answer">The contributors provide a list of their top 5 commentaries only for the books to which they are qualified to speak.</p>
      </li>
      <li class="question-item">
        <h3 class="question">What qualifies a person to contribute to Scholars Roundtable?</h3>
        <p class="answer">
          Scholars Roundtable seeks out contributors by <strong>invitation only</strong> who have
          <ul>
            <li class="dashed">
              <span>taught a particular book multiple times at the master’s level or higher,</span>
            </li>
            <li class="dashed">
              <span>who have completed their master’s or doctoral thesis on a given area, or</span>
            </li>
            <li class="dashed">
              <span>who have a sufficient amount of exegetical experience in a given book of the Bible that they are deemed qualified to speak on the best resources (for instance, they have written a commentary on the book, they have intensively advised other’s doctoral theses, or they have written extensive journal articles or books on a particular book).</span>
            </li>
          </ul>
        </p>
      </li>
      <li class="question-item">
        <h3 class="question">Can a contributor rank their own commentary?</h3>
        <p class="answer">In order to keep the data unbiased, a contributor is not allowed to rank their own commentary, however, our algorithm is designed to account for this variance. Our algorithm protects a commentary’s ranking from being negatively affected by their inability to rank it.</p>
      </li>
      <li class="question-item">
        <h3 class="question">If a commentary is not in the top 5 does that mean it is a bad resource?</h3>
        <p class="answer">No. The top resources have received the highest and most consistent rankings by the scholars. We list resources beyond the top 5 since these resources have come highly recommended, just not as extensively as the top 5. Our rankings should only be viewed as positive.</p>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
// Vue
import { Vue } from 'vue-property-decorator';

export default class Faq extends Vue {}
</script>

<style scoped lang="scss">
@import '@/styles/page-content';

.question-item {
  list-style: none;
}

.answer {
  padding-left: 2rem;
}

// .question {
// }
</style>