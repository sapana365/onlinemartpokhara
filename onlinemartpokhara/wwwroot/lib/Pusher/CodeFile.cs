 < script >
    var pusher = new Pusher('XXX_APP_KEY', {
        cluster: 'XXX_APP_CLUSTER'
    });
var my_channel = pusher.subscribe('asp_channel');
var app = new Vue({
        el: '#app',
    data: {

        comments: [],
        comment: {

            Name: '',
            Body: '',
            ProductsId: @Model.ProductsId
            }
        },
        created: function() {
    this.get_comments();
    this.listen();
},
        methods:
{
get_comments: function() {
        axios.get('@Url.Action("Review", "Home", new { id = @Model.ProductsId }, protocol: Request.Url.Scheme)')
            .then((response) => {

                this.comments = response.data;

            });

    },
            listen: function() {
        my_channel.bind("asp_event", (data) => {
            if (data.ProductsId == this.comment.ProductsId)
            {
                this.comments.push(data);
            }

        })
            },
            submit_comment: function() {
        axios.post('@Url.Action("Review", "Home", new {}, protocol: Request.Url.Scheme)', this.comment)
            .then((response) => {
                this.comment.Name = '';
                this.comment.Body = '';
                alert("Comment Submitted");

            });
    }
}
    });
    </ script >